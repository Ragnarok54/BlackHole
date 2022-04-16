using BlackHole.Domain.DTO.Message;
using BlackHole.Domain.DTO.User;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHole.Business.Services
{
    public class ConversationService : BaseService
    {
        public ConversationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<ConversationSnapshot> GetSnapshots(Guid userId, int count, int skip)
        {
            return UnitOfWork.ConversationRepository.GetLatestConversations(userId, count, skip)
                                                    .Select(c => new ConversationSnapshot
                                                    {
                                                        ConversationId = c.ConversationId,
                                                        Name = c.Name,
                                                        Text = c.LastMessage?.Text,
                                                        LastMessageTime = c.LastMessage?.UpdatedOn ?? c.LastMessage?.CreatedOn,
                                                        Highlight = !c.LastMessage?.Seen ?? false
                                                    });
        }

        public bool BelongsToConversation(Guid conversationId, Guid userId)
        {
            return UnitOfWork.ConversationRepository.GetUserConversations(userId).Any(c => c.ConversationId == conversationId);
        }

        public IEnumerable<Guid> GetConversationUsers(Guid conversationId)
        {
            return UnitOfWork.ConversationRepository.GetConversationUsers(conversationId);
        }

        public Conversation AddConversation(string name)
        {
            var conversation = new Conversation
            {
                Name = name,            
            };

            UnitOfWork.ConversationRepository.Add(conversation);

            Save();

            return conversation;
        }

        public UserConversation AddUser(Guid conversationId, Guid userId)
        {
            var userConversation = new UserConversation
            {
                ConversationId = conversationId,
                UserId = userId
            };

            UnitOfWork.UserConversationRepository.Add(userConversation);

            Save();

            return userConversation;
        }

        public void RemoveUser(Guid conversationId, Guid userId)
        {
            var userConversation = UnitOfWork.UserConversationRepository.Find(uc => uc.ConversationId == conversationId && uc.UserId == userId).First();

            UnitOfWork.UserConversationRepository.Remove(userConversation);

            Save();
        }

        public IEnumerable<MessageModel> GetMessages(Guid conversationId, int skip, int take)
        {
            return UnitOfWork.MessageRepository.GetMessages(conversationId, skip, take)
                                               .Select(m => new MessageModel
                                               {
                                                   ConversationId = m.ConversationId,
                                                   UserId = m.SenderUserId,
                                                   MessageId = m.MessageId,
                                                   Text = m.Text,
                                               });
        }

        public string GetConversationName(Guid conversationId)
        {
            return UnitOfWork.ConversationRepository.Get(conversationId).Name;
        }

        public IEnumerable<UserModel> GetContacts(Guid userId, string query)
        {
            return UnitOfWork.ConversationRepository.GetContacts(userId, query)
                                                    .Select(u => new UserModel
                                                    {
                                                        UserId = u.UserId,
                                                        FirstName = u.FirstName,
                                                        LastName = u.LastName,
                                                        PhoneNumber = u.PhoneNumber,
                                                    });
        }

        public void MarkConversationAsSeen(Guid conversationId, Guid currentUserId)
        {
            var messages = UnitOfWork.MessageRepository.GetUnseenMessages(conversationId, currentUserId);

            messages.ToList().ForEach(u => u.Seen = true);

            Save();
        }
    }
}
