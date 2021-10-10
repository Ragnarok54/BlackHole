using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHole.Domain.Entities
{
    [Table("tblAttachmentType")]
    public class AttachmentType
    {
        [Key]
        public int AttachmentTypeId { get; set; }

        [Required]
        public string Type { get; set; }

        
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
