using System;

namespace BlackHole.Domain.DTO.Message
{
    public class ConversationSnapshot
    {
        public Guid ConversationId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
