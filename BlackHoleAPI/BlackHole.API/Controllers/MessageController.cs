using BlackHole.API.Authorization;
using BlackHole.API.Hubs;
using BlackHole.Business.Services;
using BlackHole.Common;
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
        [BlackHoleAuthorize]
        public IActionResult Send([FromBody] BaseMessageModel model)
        {
            try
            {
                if (_conversationService.BelongsToConversation(model.ConversationId, CurrentUserId))
                {
                    var message = _messageService.Send(model, CurrentUserId);

                    var usersToNotify = _conversationService.GetConversationUsers(model.ConversationId);
                    usersToNotify = usersToNotify.Where(u => u.UserId != CurrentUserId);

                    foreach (var user in usersToNotify)
                    {
                        _hubContext.Clients.User(user.UserId.ToString()).SendAsync(Constants.ReceiveHubMessageMethod, message);
                    }

                    return Ok(message);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to send message for " + CurrentUserId + "\nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
