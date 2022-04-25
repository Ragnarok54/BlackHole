using BlackHole.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHole.Domain.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        IEnumerable<Message> GetMessages(Guid conversationId, int skip, int take);
        IEnumerable<Message> GetUnseenMessages(Guid conversationId, Guid currentUserId);
    }
}
