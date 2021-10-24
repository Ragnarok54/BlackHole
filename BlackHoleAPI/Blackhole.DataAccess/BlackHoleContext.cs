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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                        .HasOne(m => m.FromUser)
                        .WithMany(u => u.ReceivedMessages)
                        .HasForeignKey(m => m.FromUserId)
                        .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Message>()
                        .HasOne(m => m.ToUser)
                        .WithMany(u => u.SentMessages)
                        .HasForeignKey(m => m.ToUserId)
                        .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
