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
            var userConversations = GetUserConversations(userId);

            return userConversations.OrderByDescending(c => c.LastMessage.UpdatedOn)
                                    .ThenByDescending(c => c.LastMessage.CreatedOn)
                                    .Skip(skip)
                                    .Take(count)
                                    .ToList();
        }

        public IQueryable<Conversation> GetUserConversations(Guid userId)
        {
            return _context.UserConversations.Include(uc => uc.Conversation)
                                             .ThenInclude(c => c.LastMessage)
                                             .Where(uc => uc.UserId == userId)
                                             .Select(uc => uc.Conversation);
        }

        public IEnumerable<Guid> GetConversationUsers(Guid conversationId)
        {
            return _context.UserConversations.Where(uc => uc.ConversationId == conversationId)
                                             .Select(uc => uc.UserId)
                                             .ToList();
        }

        public IEnumerable<User> GetContacts(Guid userId, string query)
        {
            var conversations = GetUserConversations(userId).Select(c => c.ConversationId);
            
            return _context.UserConversations.Include(uc => uc.User)
                                             .Where(uc => conversations.Contains(uc.ConversationId))
                                             .Select(uc => uc.User)
                                             .Where(u => u.UserId != userId && (u.FirstName + " " + u.LastName).Contains(query))
                                             .Distinct();
        }
    }
}
