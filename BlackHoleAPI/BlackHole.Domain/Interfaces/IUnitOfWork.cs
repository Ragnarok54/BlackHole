using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces.Repositories;

namespace BlackHole.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Attachment> AttachmentRepository { get; }
        IRepository<AttachmentType> AttachmentTypeRepository { get; }
        IRepository<Message> MessageRepository { get; }
        IUserRepository UserRepository { get; }

        int SaveChanges();
    }
}
