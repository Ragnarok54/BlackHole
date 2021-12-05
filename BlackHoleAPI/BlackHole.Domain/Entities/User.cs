using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHole.Domain.Entities
{
    [Table("tblUser")]
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [RegularExpression(@"^07\d{2}\s?\d{3}\s?\d{3}$")]
        public string PhoneNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public DateTime LastLoginDate { get; set; }

        public byte[] Picture { get; set; }


        public virtual ICollection<Conversation> Conversations { get; set; }
    }
}
