using BlackHole.API.Authorization;
using BlackHole.Business.Services;
using BlackHole.Domain.DTO.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace BlackHole.API.Controllers
{
    [ApiController]
    [BlackHoleAuthorize]
    [Route("/api/[controller]s")]
    public class MessageController : BaseController
    {
        private readonly MessageService _messageService;
        private readonly ConversationService _conversationService;
        private readonly ILogger<ConversationController> _logger;

        public MessageController(MessageService messageService, ConversationService conversationService, ILogger<ConversationController> logger)
        {
            _messageService = messageService;
            _conversationService = conversationService;
            _logger = logger;
        }


        [HttpPost]
        [Route("Send")]
        public IActionResult Send([FromBody] MessageModel message)
        {
            try
            {
                if (_conversationService.BelongsToConversation(message.ConversationId, (Guid)CurrentUserId))
                {
                    _messageService.Send(message, (Guid)CurrentUserId);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to fetch conversation snapshots for " + CurrentUserId + "\nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
