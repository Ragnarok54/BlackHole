using System;


namespace BlackHole.Domain.DTO.Conversation
{
    public class ConversationMessage
    {
        public Guid ConversationId { get; set; }
        public string Text { get; set; }
    }
}
