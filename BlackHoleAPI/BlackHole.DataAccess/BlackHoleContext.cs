using BlackHole.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlackHole.DataAccess
{
    public class BlackHoleContext : DbContext
    {
        public BlackHoleContext(DbContextOptions<BlackHoleContext> options) : base(options) { }


        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AttachmentType> AttachmentTypes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<UserConversation> UserConversations { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                        .HasOne(c => c.Conversation)
                        .WithOne(m => m.LastMessage)
                        .HasForeignKey<Conversation>(m => m.LastMessageId)
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                        .HasOne(m => m.RepliedMessage)
                        .WithMany(m => m.Replies)
                        .HasForeignKey(m => m.RepliedMessageId)
                        .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
