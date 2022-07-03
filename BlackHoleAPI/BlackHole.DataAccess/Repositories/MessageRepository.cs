using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BlackHole.DataAccess.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly string _key;
        private readonly string _salt;

        public MessageRepository(BlackHoleContext context, IConfiguration configuration) : base(context)
        {
            _key = configuration["AppSettings:MessageEncryptionKey"];
            _salt = configuration["AppSettings:MessageEncryptionSalt"];
        }

        public override void Add(Message message)
        {
            message.Text = Encrypt(message.Text);

            base.Add(message);
        }

        public IEnumerable<Message> GetMessages(Guid conversationId, int skip, int take)
        {
            return GetConversationMessages(conversationId).OrderByDescending(m => m.CreatedOn)
                                                          .Skip(skip)
                                                          .Take(take)
                                                          .AsEnumerable()
                                                          .Select(m => DecryptMessage(m))
                                                          .ToList();
        }

        public IEnumerable<Message> GetUnseenMessages(Guid conversationId, Guid currentUserId)
        {
            return GetConversationMessages(conversationId).Where(m => m.SenderUserId != currentUserId).ToList();
        }

        private IQueryable<Message> GetConversationMessages(Guid conversationId)
        {
            return _context.Messages.Include(m => m.RepliedMessage)
                                    .Where(m => m.ConversationId == conversationId);
        }

        private Message DecryptMessage(Message message)
        {
            if (message == null)
            {
                return null;
            }

            var decryptedMessage = message;

            decryptedMessage.Text = Decrypt(message.Text);
            
            if (decryptedMessage.RepliedMessage != null)
            {
                decryptedMessage.RepliedMessage.Text = Decrypt(message.RepliedMessage.Text);
            }

            return decryptedMessage;
        }

        private string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(_key, Encoding.ASCII.GetBytes(_salt));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(_key, Encoding.ASCII.GetBytes(_salt));
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
