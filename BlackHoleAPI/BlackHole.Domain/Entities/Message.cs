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

        [Required]
        public int FromUserId { get; set; }
        
        [Required]
        public int ToUserId { get; set; }

        public string Text { get; set; }
        
        public Guid? AttachmentId { get; set; }

        [Required]
        public bool Seen { get; set; }


        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
