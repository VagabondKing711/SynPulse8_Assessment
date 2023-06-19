using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_Identity
{
    public class IdentityConfigurator
    {
        private static string _issuer = "SynPulse8.Bearer";
        private static string _audience = "SynPulse8.Bearer";
        private static SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(@"{signingKey}"));
        private static string _securityAlgorithm = SecurityAlgorithms.HmacSha256;
        private static uint _ttl = 15;

        public static void SynPulse8JwtBearer(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = _signingKey
            };
        }

        public static string CreateAccessToken(List<Claim>? claims = null)
        {

            if (claims == null)
            {
                claims = new List<Claim>();
            }

            var token = new JwtSecurityToken
            (
                issuer: _issuer,
                audience: _audience,
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(_ttl),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(_signingKey, _securityAlgorithm)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
