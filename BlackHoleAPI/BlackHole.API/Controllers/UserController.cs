using BlackHole.API.Authorization;
using BlackHole.Business.Services;
using BlackHole.Domain.DTO.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace BlackHole.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var user = _userService.Login(model.PhoneNumber, model.Password);

                if (user != null)
                {
                    return Ok(new LoginResponseModel
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Picture = user.Picture != null ? Convert.ToBase64String(user.Picture) : null,
                        Token = JwtService.GenerateToken(user)
                    });
                }

                return Forbid();
            }
            catch
            {
                return Forbid();
            }
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            try
            {
                var result = _userService.Register(model);

                return new JsonResult(result != null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user ");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [BlackHoleAuthorize]
        public IActionResult Details(string userId)
        {
            try
            {
                var user = _userService.GetUser(new Guid(userId));

                return new JsonResult(new UserModel
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Picture = user.Picture == null ? null : Convert.ToBase64String(user.Picture)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering user ");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("Edit")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [BlackHoleAuthorize]
        public IActionResult Edit([FromBody] UserModel model)
        {
            try
            {
                _userService.Edit(model, CurrentUserId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("Picture/{userId}")]
        [BlackHoleAuthorize]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Picture(Guid userId)
        {
            try
            {
                return Ok(_userService.GetPicture(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching picture");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
