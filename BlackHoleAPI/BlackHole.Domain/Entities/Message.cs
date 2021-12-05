using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHole.Domain.Entities
{
    [Table("tblMessage")]
    public class Message
    {
        [Key]
        public Guid MessageId { get; set; }
        public Guid ConversationId { get; set; }
        public Guid? AttachmentId { get; set; }
        public string Text { get; set; }
        public Guid SenderUserId { get; set; }
        public DateTime CreatedOn { get; set;}
        public DateTime? UpdatedOn { get; set; }
        public bool Seen { get; set; }


        public virtual Conversation Conversation { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual User SenderUser { get; set; }
    }
}
