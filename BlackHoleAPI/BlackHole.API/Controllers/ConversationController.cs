using BlackHole.API.Authorization;
using BlackHole.Business.Services;
using BlackHole.Domain.DTO.Conversation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace BlackHole.API.Controllers
{
    [ApiController]
    public class ConversationController : BaseController
    {
        private readonly ConversationService _conversationervice;
        private readonly ILogger<ConversationController> _logger;

        public ConversationController(ConversationService conversationervice, ILogger<ConversationController> logger)
        {
            _conversationervice = conversationervice;
            _logger = logger;
        }

        [HttpGet]
        [Route("/api/[controller]s")]
        [BlackHoleAuthorize]
        public IActionResult Index([FromQuery] int count, int skip)
        {
            var userId = GetCurrentUserId();

            try
            {
                var conversationSnapshots = _conversationervice.GetSnapshots((Guid)userId, count, skip);

                return new JsonResult(conversationSnapshots);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to fetch conversation snapshots for " + userId + "\nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("/api/[controller]/Add")]
        [BlackHoleAuthorize]
        [ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] ConversationModel model)
        {
            var currentUserId = GetCurrentUserId();

            try
            {
                var conversation = _conversationervice.AddConversation(model.Name);
                _conversationervice.AddUser(conversation.ConversationId, (Guid)currentUserId);

                foreach (var userId in model.UserIds)
                {
                    _conversationervice.AddUser(conversation.ConversationId, userId);
                }

                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to add conversation for {currentUserId} \nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Adds an user to a conversation
        /// </summary>
        /// <param name="conversationId">Conversation to which the user should be added</param>
        /// <param name="userId">User to be added</param>
        [HttpPost]
        [Route("/api/[controller]/AddUser")]
        [BlackHoleAuthorize]
        [ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status403Forbidden), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddUser([FromQuery] Guid conversationId, Guid userId)
        {
            var currentUserId = GetCurrentUserId();

            try
            {
                if (!_conversationervice.BelongsToConversation(conversationId, (Guid)currentUserId))
                {
                    return StatusCode((int)HttpStatusCode.Forbidden);
                }

                _conversationervice.AddUser(conversationId, userId);


                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to add user {userId} to conversation {conversationId} by {currentUserId} \nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("/api/[controller]/RemoveUser")]
        [BlackHoleAuthorize]
        public IActionResult RemoveUser([FromQuery] Guid conversationId, Guid userId)
        {
            var currentUserId = GetCurrentUserId();

            try
            {
                if (!_conversationervice.BelongsToConversation(conversationId, (Guid)currentUserId)
                    || !_conversationervice.BelongsToConversation(conversationId, userId))
                {
                    return StatusCode((int)HttpStatusCode.Forbidden);
                }

                _conversationervice.RemoveUser(conversationId, userId);


                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to remove user {userId} from conversation {conversationId} by {currentUserId} \nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
