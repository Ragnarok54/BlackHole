using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BlackHole.UnitTests.TestRepositories
{
    public class TestConversationRepository : IConversationRepository
    {
        private readonly List<Conversation> _conversations;

        public TestConversationRepository()
        {
            _conversations = new List<Conversation>
            {
                new Conversation
                {

                },
            };
        }

        public void Add(Conversation entity)
        {
            _conversations.Add(entity);
        }

        public void AddRange(IEnumerable<Conversation> entities)
        {
            _conversations.AddRange(entities);
        }

        public IEnumerable<Conversation> Find(Expression<Func<Conversation, bool>> predicate)
        {
            return _conversations.Where(predicate.Compile());
        }

        public IEnumerable<Conversation> GetLatestConversations(Guid userId, int count, int skip)
        {
            var userConversations = GetUserConversations(userId);

            return userConversations.OrderByDescending(c => c.LastMessage.UpdatedOn)
                                    .ThenByDescending(c => c.LastMessage.CreatedOn)
                                    .Skip(skip)
                                    .Take(count);
        }

        public IQueryable<Conversation> GetUserConversations(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void Remove(Conversation entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Conversation> entities)
        {
            throw new NotImplementedException();
        }

        public Conversation SingleOrDefault(Expression<Func<Conversation, bool>> predicate)
        {
            return _conversations.SingleOrDefault(predicate.Compile());
        }

        public Conversation Get(int id)
        {
            throw new NotImplementedException();
        }

        public Conversation Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Conversation> GetAll()
        {
            return _conversations;
        }

        public IEnumerable<User> GetConversationUsers(Guid conversationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetContacts(Guid userId, string query)
        {
            throw new NotImplementedException();
        }

        public string GetConversationName(Conversation conversation, Guid currentUserId)
        {
            throw new NotImplementedException();
        }

        public string GetConversationPicture(Conversation conversation, Guid currentUserId)
        {
            throw new NotImplementedException();
        }

        public Conversation Get(Guid user1, Guid user2)
        {
            throw new NotImplementedException();
        }
    }
}
