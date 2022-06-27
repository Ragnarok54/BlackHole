using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackHole.Domain.Entities
{
    [Table("tblConversation")]
    public class Conversation
    {
        [Key]
        public Guid ConversationId { get; set; }
        public string Name { get; set; }
        public Guid? LastMessageId { get; set; }


        public virtual Message LastMessage { get; set; }
        public virtual ICollection<UserConversation> UserConversations { get; set; }
    }
}
