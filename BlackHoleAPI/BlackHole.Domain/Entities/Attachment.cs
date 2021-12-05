using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHole.Domain.Entities
{
    [Table("tblAttachment")]
    public class Attachment
    {
        [Key]
        public Guid AttachmentId { get; set; }

        [Required]
        public byte[] Obejct { get; set; }

        [Required]
        public int AttachamentTypeId { get; set; }


        public virtual AttachmentType AttachmentType { get; set; }
    }
}
