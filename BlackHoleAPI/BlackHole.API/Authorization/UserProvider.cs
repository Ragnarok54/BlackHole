using BlackHole.Business.Services;
using BlackHole.Common.Enums;
using Microsoft.AspNetCore.SignalR;
using System;

namespace BlackHole.API.Authorization
{
    public class UserProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var token = connection.GetHttpContext().Request.Query["access_token"].ToString();

            if (token != null)
            {
                return JwtService.GetClaim(TokenClaim.UserId, token);
            }
            else
            {
                return null;
            }
        }
    }
}
