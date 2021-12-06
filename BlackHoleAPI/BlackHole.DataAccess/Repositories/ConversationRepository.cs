using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHole.DataAccess.Repositories
{
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        public ConversationRepository(BlackHoleContext context) : base(context) { }


        public IEnumerable<Conversation> GetLatestConversations(Guid userId, int count, int skip)
        {
            var userConversations = GetUserConversations(userId).Include(c => c.LastMessage);

            return userConversations.OrderByDescending(c => c.LastMessage.UpdatedOn)
                                    .ThenByDescending(c => c.LastMessage.CreatedOn)
                                    .Skip(skip)
                                    .Take(count);
        }

        public IQueryable<Conversation> GetUserConversations(Guid userId)
        {
            return _context.UserConversations.Where(uc => uc.UserId == userId).Select(uc => uc.Conversation);
        }
    }
}
