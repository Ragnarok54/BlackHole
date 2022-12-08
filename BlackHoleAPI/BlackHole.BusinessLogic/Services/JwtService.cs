using BlackHole.Common;
using BlackHole.Common.Enums;
using BlackHole.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace BlackHole.Business.Services
{
    /// <summary>
    /// JSON Web Token Service
    /// </summary>
    public static class JwtService
    {
        /// <summary>
        /// Generate a new token for a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(((int)TokenClaim.UserId).ToString(), user.UserId.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Constants.JwtExpireTimeInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Settings.TokenSecretBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Get the claims from a token
        /// </summary>
        /// <param name="claimKey"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetClaim(TokenClaim claimKey, string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenSecure = handler.ReadToken(token) as JwtSecurityToken;

            return tokenSecure.Claims.First(claim => claim.Type == ((int)claimKey).ToString()).Value;
        }
    }
}
