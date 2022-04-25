using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using BlackHole.Domain.Interfaces.Repositories;
using System;

namespace BlackHole.UnitTests.TestRepositories
{
    public class TestUnitOfWork : IUnitOfWork
    {
        private bool isDisposed;

        private IUserRepository _userRepository;
        private IConversationRepository _conversationRepository;

        public IRepository<Attachment> AttachmentRepository => throw new NotImplementedException();
        public IRepository<AttachmentType> AttachmentTypeRepository => throw new NotImplementedException();
        public IMessageRepository MessageRepository => throw new NotImplementedException();
        public IUserRepository UserRepository => _userRepository ??= new TestUserRepository();

        public IConversationRepository ConversationRepository => _conversationRepository ??= new TestConversationRepository();

        public IRepository<UserConversation> UserConversationRepository => throw new NotImplementedException();


        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public int SaveChanges() => 0;
    }
}
