using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using BlackHole.Domain.Interfaces.Repositories;
using System;

namespace BlackHole.UnitTests.TestRepositories
{
    public class TestUnitOfWork : IUnitOfWork
    {
        private IUserRepository _userRepository;


        public IRepository<Attachment> AttachmentRepository => throw new NotImplementedException();
        public IRepository<AttachmentType> AttachmentTypeRepository => throw new NotImplementedException();
        public IRepository<Message> MessageRepository => throw new NotImplementedException();
        public IUserRepository UserRepository => _userRepository ??= new TestUserRepository();

        public IConversationRepository ConversationRepository => throw new NotImplementedException();

        public IRepository<UserConversation> UserConversationRepository => throw new NotImplementedException();


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges() => 0;
    }
}
