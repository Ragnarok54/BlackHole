using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BlackHole.UnitTests.TestRepositories
{
    public class TestUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public TestUserRepository()
        {
            _users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    PhoneNumber = "0712 345 678",
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    LastLoginDate = DateTime.Now,
                    Salt = "salt",
                    PasswordHash = ""
                }
            };
        }

        public void Add(User entity)
        {
            _users.Add(entity);
        }

        public void AddRange(IEnumerable<User> entities)
        {
            _users.AddRange(entities);
        }

        public IEnumerable<User> Find(Expression<Func<User, bool>> predicate)
        {
            return _users.Where(predicate.Compile());
        }

        public User Get(string phoneNumber)
        {
            return _users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        }

        public User Get(int id)
        {
            return _users.FirstOrDefault(u => u.UserId == id);
        }

        public User Get(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(User entity)
        {
            _users.Remove(entity);
        }

        public void RemoveRange(IEnumerable<User> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public User SingleOrDefault(Expression<Func<User, bool>> predicate)
        {
            return _users.SingleOrDefault(predicate.Compile());
        }
    }
}
