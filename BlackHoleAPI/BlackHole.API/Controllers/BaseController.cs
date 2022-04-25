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

        /// <summary>
        /// Id of the current user
        /// </summary>
        /// <remarks>Do not use in anonymously accesible endpoints</remarks>
        private protected Guid CurrentUserId
        {
            get
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token == null)
                {
                    throw new InvalidOperationException("Current user not available due to null token");
                }

                return Guid.Parse(JwtService.GetClaim(TokenClaim.UserId, token));
            }
        }
    }
}
