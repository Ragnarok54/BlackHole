using BlackHole.API.Authorization;
using BlackHole.API.Hubs;
using BlackHole.Business.Services;
using BlackHole.Domain.DTO.Message;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;

namespace BlackHole.API.Controllers
{
    [ApiController]
    [BlackHoleAuthorize]
    [Route("/api/[controller]s")]
    public class MessageController : BaseController
    {
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly MessageService _messageService;
        private readonly ConversationService _conversationService;
        private readonly ILogger<ConversationController> _logger;

        public MessageController(IHubContext<MessageHub> hubContext, MessageService messageService, ConversationService conversationService, ILogger<ConversationController> logger)
        {
            _hubContext = hubContext;
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

                    var usersToNotify = _conversationService.GetConversationUsers(message.ConversationId);
                    usersToNotify = usersToNotify.Where(u => u != (Guid)CurrentUserId);

                    foreach (var user in usersToNotify)
                    {
                        _hubContext.Clients.User(user.ToString()).SendAsync("ReceiveOne", message.ConversationId, message.Text);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to send message for " + CurrentUserId + "\nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
