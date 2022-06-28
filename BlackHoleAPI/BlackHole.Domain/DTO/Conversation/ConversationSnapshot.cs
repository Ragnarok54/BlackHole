using System;

namespace BlackHole.Domain.DTO.Message
{
    public class ConversationSnapshot
    {
        public Guid ConversationId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public BaseMessageModel LastMessage { get; set; }
    }
}
