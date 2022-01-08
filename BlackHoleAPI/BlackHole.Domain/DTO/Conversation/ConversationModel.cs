using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlackHole.Domain.DTO.Message
{
    public class ConversationModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public IEnumerable<Guid> UserIds { get; set; }
    }
}
