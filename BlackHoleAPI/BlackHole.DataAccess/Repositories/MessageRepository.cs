using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHole.DataAccess.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(BlackHoleContext context) : base(context) { }

        public IEnumerable<Message> GetMessages(Guid conversationId, int skip, int take)
        {
            return GetConversationMessages(conversationId).OrderByDescending(m => m.CreatedOn)
                                                          .Skip(skip)
                                                          .Take(take)
                                                          .AsEnumerable();
        }

        public IEnumerable<Message> GetUnseenMessages(Guid conversationId, Guid currentUserId)
        {
            return GetConversationMessages(conversationId).Where(m => m.SenderUserId != currentUserId);
        }

        private IQueryable<Message> GetConversationMessages(Guid conversationId)
        {
            return _context.Messages.Where(m => m.ConversationId == conversationId);
        }
    }
}
