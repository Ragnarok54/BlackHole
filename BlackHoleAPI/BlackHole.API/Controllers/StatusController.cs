using BlackHole.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace BlackHole.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;

        public StatusController(ILogger<StatusController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Version"), AllowAnonymous]
        public IActionResult Version()
        {
            try
            {
                return new JsonResult(new { ApiVersion = Settings.Version });
            }
            catch
            {
                _logger.LogError("Failed to get API version");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
