using BlackHole.Business.Services;
using BlackHole.Domain.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace BlackHole.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
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
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var user = _userService.Login(model.PhoneNumber, model.Password);

                if (user != null)
                {
                    return Ok(new LoginResponseModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
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
    }
}
