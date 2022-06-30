using BlackHole.Common;
using BlackHole.Domain.DTO.Message;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHole.Business.Services
{
    public class MessageService : BaseService
    {
        public MessageService(IUnitOfWork unitOfWork) : base(unitOfWork) { }


        public MessageModel Send(BaseMessageModel messageModel, Guid userId)
        {
            var conversation = UnitOfWork.ConversationRepository.Get(messageModel.ConversationId);
            var message = new Message
            {
                ConversationId = messageModel.ConversationId,
                RepliedMessageId = messageModel.RepliedMessage?.MessageId,
                Text = messageModel.Text,
                CreatedOn = DateTime.Now,
                SenderUserId = userId,
                Seen = false,  
            };
            conversation.LastMessage = message;

            UnitOfWork.MessageRepository.Add(message);

            Save();

            // The seen field refers to wether the current user has seen the message,
            // which is always true, since he sent it
            return new MessageModel
            {
                ConversationId = message.ConversationId,
                UserId = message.SenderUserId,
                MessageId = message.MessageId,
                Text = messageModel.Text,
                Seen = false,
                RepliedMessage = message.RepliedMessageId == null ? null : new MessageModel
                {
                    MessageId = message.RepliedMessageId,
                    UserId = UnitOfWork.MessageRepository.Get((Guid)message.RepliedMessageId).SenderUserId,
                    Text = UnitOfWork.MessageRepository.Get((Guid)message.RepliedMessageId).Text,
                },
            };
        }
        
        public void Update(BaseMessageModel messageModel)
        {
            var existingMessage = UnitOfWork.MessageRepository.Get((Guid)messageModel.MessageId); 

            existingMessage.Text = messageModel.Text;
            existingMessage.UpdatedOn = DateTime.Now;         

            Save();
        }

        public bool DeleteMessage(BaseMessageModel messageModel)
        {
            var success = false;
            var message = UnitOfWork.MessageRepository.Get((Guid)messageModel.MessageId);

            if (message.CreatedOn < DateTime.Now.AddMinutes(Constants.RemoveMessageTimeInMinutes))
            {
                message.Text = Constants.RemovedMessageText;
                message.UpdatedOn = DateTime.Now;

                Save();
                success = true;
            }

            return success;
        }
    }
}
