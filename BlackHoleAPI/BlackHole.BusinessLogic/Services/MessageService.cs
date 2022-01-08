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


        public void Send(MessageModel messageModel, Guid userId)
        {
            var message = new Message
            {
                ConversationId = messageModel.ConversationId,
                Text = messageModel.Text,
                CreatedOn = DateTime.Now,
                SenderUserId = userId,
                Seen = false,
            };

            UnitOfWork.MessageRepository.Add(message);

            Save();
        }
        
        public void Update(MessageModel messageModel)
        {
            var existingMessage = UnitOfWork.MessageRepository.Get((Guid)messageModel.MessageId); 

            existingMessage.Text = messageModel.Text;

            Save();
        }

        public bool DeleteMessage(MessageModel messageModel)
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
