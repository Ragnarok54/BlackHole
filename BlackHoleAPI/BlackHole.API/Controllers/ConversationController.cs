using BlackHole.API.Authorization;
using BlackHole.Business.Services;
using BlackHole.Domain.DTO.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;

namespace BlackHole.API.Controllers
{
    [ApiController]
    public class ConversationController : BaseController
    {
        private readonly ConversationService _conversationService;
        private readonly ILogger<ConversationController> _logger;

        public ConversationController(ConversationService conversationervice, ILogger<ConversationController> logger)
        {
            _conversationService = conversationervice;
            _logger = logger;
        }

        [HttpGet]
        [Route("/api/[controller]s")]
        [BlackHoleAuthorize]
        public IActionResult Index([FromQuery] int count, int skip)
        {
            try
            {
                var conversationSnapshots = _conversationService.GetSnapshots(CurrentUserId, count, skip);

                return Ok(conversationSnapshots);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to fetch conversation snapshots for " + CurrentUserId + "\nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/details/{conversationId}")]
        [BlackHoleAuthorize]
        public IActionResult Details(Guid conversationId)
        {
            try
            {
                if (_conversationService.BelongsToConversation(conversationId, CurrentUserId))
                {
                    var details = _conversationService.GetConversationDetails(conversationId);

                    details.UserIds = details.UserIds.Where(u => u != CurrentUserId);

                    return new JsonResult(details);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to fetch conversation snapshots for " + CurrentUserId + "\nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/{conversationId}/{skip}/{take}")]
        [BlackHoleAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status403Forbidden), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Messages(Guid conversationId, int skip, int take)
        {
            try
            {
                if (_conversationService.BelongsToConversation(conversationId, CurrentUserId))
                {
                    return Ok(_conversationService.GetMessages(conversationId, skip, take));
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to fetch messages for {CurrentUserId} and {conversationId}\nError: {ex}");

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("/api/[controller]/Add")]
        [BlackHoleAuthorize]
        [ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] ConversationModel model)
        {
            try
            {
                var conversation = _conversationService.AddConversation(model.Name);
                _conversationService.AddUser(conversation.ConversationId, CurrentUserId);

                foreach (var userId in model.UserIds)
                {
                    _conversationService.AddUser(conversation.ConversationId, userId);
                }

                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to add conversation for {CurrentUserId} \nError: " + ex);

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
            try
            {
                if (!_conversationService.BelongsToConversation(conversationId, CurrentUserId))
                {
                    return StatusCode((int)HttpStatusCode.Forbidden);
                }

                _conversationService.AddUser(conversationId, userId);


                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to add user {userId} to conversation {conversationId} by {CurrentUserId} \nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("/api/[controller]/RemoveUser")]
        [BlackHoleAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status403Forbidden), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RemoveUser([FromQuery] Guid conversationId, Guid userId)
        {
            try
            {
                if (!_conversationService.BelongsToConversation(conversationId, CurrentUserId)
                    || !_conversationService.BelongsToConversation(conversationId, userId))
                {
                    return StatusCode((int)HttpStatusCode.Forbidden);
                }

                _conversationService.RemoveUser(conversationId, userId);


                return StatusCode((int)HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to remove user {userId} from conversation {conversationId} by {CurrentUserId} \nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet]
        [Route("/api/[controller]/Contacts")]
        [BlackHoleAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Contacts([FromQuery] string query)
        {
            try
            {
                query ??= string.Empty;

                var contacts = _conversationService.GetContacts(CurrentUserId, query);


                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to fetch contacts for user {CurrentUserId} \nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("/api/[controller]/Seen/{conversationId}")]
        [BlackHoleAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Seen(Guid conversationId)
        {
            try
            {
                _conversationService.MarkConversationAsSeen(conversationId, CurrentUserId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to fetch contacts for user {CurrentUserId} \nError: " + ex);

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
