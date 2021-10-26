using BlackHole.DataAccess.Repositories;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using BlackHole.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlackHole.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlackHoleContext _context;

        private bool isDisposed;

        private IRepository<Attachment> _attachmentRepository;
        private IRepository<AttachmentType> _attachmentTypeRepository;
        private IRepository<Message> _messageRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlackHoleContext>();
            optionsBuilder.UseSqlServer(connectionString);

            _context = new BlackHoleContext(optionsBuilder.Options);
        }

        public IRepository<Attachment> AttachmentRepository => _attachmentRepository ??= new Repository<Attachment>(_context);

        public IRepository<AttachmentType> AttachmentTypeRepository => _attachmentTypeRepository ??= new Repository<AttachmentType>(_context);

        public IRepository<Message> MessageRepository => _messageRepository ??= new Repository<Message>(_context);

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);


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
