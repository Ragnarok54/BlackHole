using BlackHole.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blackhole.DataAccess
{
    public class BlackHoleContext : DbContext
    {
        public BlackHoleContext(DbContextOptions<BlackHoleContext> options) : base(options) { }


        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
