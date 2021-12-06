using BlackHole.Business.Services;
using BlackHole.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BlackHole.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController() { }

        private protected Guid? GetCurrentUserId()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                return Guid.Parse(JwtService.GetClaim(TokenClaim.UserId, token));
            }
            else
            {
                return null;
            }
        }
    }
}
