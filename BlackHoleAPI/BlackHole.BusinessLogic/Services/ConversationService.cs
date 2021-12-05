using BlackHole.Domain.DTO.Conversation;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace BlackHole.Business.Services
{
    public class ConversationService : BaseService
    {
        public ConversationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public IEnumerable<ConversationSnapshot> GetSnapshots(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Conversation AddConversation(string name)
        {
            var conversation = new Conversation
            {
                Name = name,            
            };

            return conversation;
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
