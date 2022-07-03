using BlackHole.DataAccess.Repositories;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using BlackHole.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace BlackHole.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlackHoleContext _context;
        private readonly IConfiguration _configuration;

        private bool isDisposed;

        private IRepository<Attachment> _attachmentRepository;
        private IRepository<AttachmentType> _attachmentTypeRepository;
        private IMessageRepository _messageRepository;
        private IConversationRepository _conversationRepository;
        private IRepository<UserConversation> _userConversationRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(string connectionString, IConfiguration configuration)
        {
            _configuration = configuration;

            var optionsBuilder = new DbContextOptionsBuilder<BlackHoleContext>();
            optionsBuilder.UseSqlServer(connectionString);

            _context = new BlackHoleContext(optionsBuilder.Options);
        }

        public IRepository<Attachment> AttachmentRepository => _attachmentRepository ??= new Repository<Attachment>(_context);
        public IRepository<AttachmentType> AttachmentTypeRepository => _attachmentTypeRepository ??= new Repository<AttachmentType>(_context);
        public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context, _configuration);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
        public IConversationRepository ConversationRepository => _conversationRepository ??= new ConversationRepository(_context, _configuration);
        public IRepository<UserConversation> UserConversationRepository => _userConversationRepository ??= new Repository<UserConversation>(_context);


        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

    }
}
