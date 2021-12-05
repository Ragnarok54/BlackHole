using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHole.Domain.Entities
{
    [Table("tblUserConversation")]
    public class UserConversation
    {
        [Key]
        public Guid UserConversationId { get; set; }
        public int UserId { get; set; }
        public Guid ConversationId { get; set; }


        public virtual User User { get; set; }
        public virtual Conversation Conversation { get; set; }
    }
}
