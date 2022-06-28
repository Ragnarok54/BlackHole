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
            //var conversations = GetUserConversations(userId).Select(c => c.ConversationId);
            //return _context.UserConversations.Include(uc => uc.User)
            //                                 .Where(uc => conversations.Contains(uc.ConversationId))
            //                                 .Select(uc => uc.User)
            //                                 .Where(u => u.UserId != userId && ((u.FirstName + " " + u.LastName).Contains(query) || u.PhoneNumber.Contains(query)))
            //                                 .Distinct();
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
    }
}
