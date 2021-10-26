using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using BlackHole.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlackHole.UnitTests.TestRepositories
{
    public class TestUnitOfWork : IUnitOfWork
    {
        private IUserRepository _userRepository;


        public IRepository<Attachment> AttachmentRepository => throw new NotImplementedException();
        public IRepository<AttachmentType> AttachmentTypeRepository => throw new NotImplementedException();
        public IRepository<Message> MessageRepository => throw new NotImplementedException();
        public IUserRepository UserRepository => _userRepository ??= new TestUserRepository();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges() => 0;
    }
}
