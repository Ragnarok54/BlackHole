using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces.Repositories;
using System;

namespace BlackHole.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Attachment> AttachmentRepository { get; }
        IRepository<AttachmentType> AttachmentTypeRepository { get; }
        IRepository<Message> MessageRepository { get; }
        IRepository<Conversation> ConversationRepository { get; }
        IRepository<UserConversation> UserConversationRepository { get; }
        IUserRepository UserRepository { get; }

        int SaveChanges();
    }
}
