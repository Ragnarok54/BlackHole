using BlackHole.Domain.DTO.Conversation;
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
                                                        Text = c.LastMessage.Text,
                                                        LastMessageTime = c.LastMessage.UpdatedOn ?? c.LastMessage.CreatedOn
                                                    });
        }

        public bool BelongsToConversation(Guid conversationId, Guid userId)
        {
            return UnitOfWork.ConversationRepository.GetUserConversations(userId).Any(c => c.ConversationId == conversationId);
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

        public void SendMessage(ConversationMessage conversationMessage, Guid userId)
        {
            var message = new Message
            {
                ConversationId = conversationMessage.ConversationId,
                Text = conversationMessage.Text,
                CreatedOn = DateTime.Now,
                SenderUserId = userId,
                Seen = false,
            };

            UnitOfWork.MessageRepository.Add(message);
        }
    }
}
