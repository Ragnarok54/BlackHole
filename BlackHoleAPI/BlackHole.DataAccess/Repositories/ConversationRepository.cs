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
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        private readonly string _key;
        private readonly string _salt;

        public ConversationRepository(BlackHoleContext context, IConfiguration configuration) : base(context)
        {
            _key = configuration["AppSettings:MessageEncryptionKey"];
            _salt = configuration["AppSettings:MessageEncryptionSalt"];
        }


        public IEnumerable<Conversation> GetLatestConversations(Guid userId, int count, int skip)
        {
            var userConversations = GetUserConversations(userId);

            return userConversations.OrderByDescending(c => c.LastMessage == null)
                                    .ThenByDescending(c => c.LastMessage.UpdatedOn)
                                    .ThenByDescending(c => c.LastMessage.CreatedOn)
                                    .Skip(skip)
                                    .Take(count)
                                    .AsEnumerable()
                                    .Select(c => DecryptConversation(c))
                                    .ToList();
        }

        public IQueryable<Conversation> GetUserConversations(Guid userId)
        {
            return _context.UserConversations.Include(uc => uc.Conversation)
                                             .ThenInclude(c => c.LastMessage)
                                             .Where(uc => uc.UserId == userId)
                                             .Select(uc => uc.Conversation);
        }

        public Conversation Get(Guid user1, Guid user2)
        {
            return _context.Conversations.FirstOrDefault(c => c.UserConversations.Any(uc => uc.UserId == user1)
                                                           && c.UserConversations.Any(uc => uc.UserId == user2)
                                                           && c.UserConversations.Count == 2);
        }

        public IEnumerable<User> GetConversationUsers(Guid conversationId)
        {
            return _context.UserConversations.Include(uc => uc.User)
                                             .Where(uc => uc.ConversationId == conversationId)
                                             .Select(uc => uc.User)
                                             .ToList();
        }

        public IEnumerable<User> GetContacts(Guid userId, string query)
        {
            return _context.Users.Where(u => u.UserId != userId && ((u.FirstName + " " + u.LastName).Contains(query) || u.PhoneNumber.Contains(query)))
                                 .Distinct();
        }

        private User GetConversationTargetUser(Guid conversationId, Guid currentUserId)
        {
            return _context.UserConversations.Include(uc => uc.User)
                                             .First(uc => uc.ConversationId == conversationId && uc.UserId != currentUserId).User;
        }

        public string GetConversationName(Conversation conversation, Guid currentUserId)
        {
            var name = conversation.Name;

            if (string.IsNullOrEmpty(name))
            {
                var user = GetConversationTargetUser(conversation.ConversationId, currentUserId);
                name = user.FirstName + " " + user.LastName;
            }

            return name;
        }

        public string GetConversationPicture(Conversation conversation, Guid currentUserId)
        {
            var user = GetConversationTargetUser(conversation.ConversationId, currentUserId);
            return user.Picture == null ? null : Convert.ToBase64String(user.Picture);
        }

        private Conversation DecryptConversation(Conversation conversation)
        {
            var decryptedConversation = conversation;

            decryptedConversation.LastMessage = DecryptMessage(conversation.LastMessage);

            return decryptedConversation;
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
