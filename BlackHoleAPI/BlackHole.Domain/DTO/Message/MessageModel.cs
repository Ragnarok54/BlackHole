using System;

namespace BlackHole.Domain.DTO.Message
{
    public class MessageModel : BaseMessageModel
    {
        public Guid UserId { get; set; }
    }
}
