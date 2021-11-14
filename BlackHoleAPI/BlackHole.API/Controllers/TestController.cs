using BlackHole.Business.Services;
using BlackHole.Common;
using BlackHole.Domain.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace BlackHole.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public TestController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [Route("Version"), AllowAnonymous]
        public IActionResult Version()
        {
            try
            {
                return new JsonResult(Settings.Version);
            }
            catch
            {
                _logger.LogError("Failed to get version");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
