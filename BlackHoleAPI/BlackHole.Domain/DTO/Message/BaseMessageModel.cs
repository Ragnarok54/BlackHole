﻿using System;


namespace BlackHole.Domain.DTO.Message
{
    public class BaseMessageModel
    {
        public Guid ConversationId { get; set; }
        public Guid? MessageId { get; set; }
        public string Text { get; set; }
    }
}
