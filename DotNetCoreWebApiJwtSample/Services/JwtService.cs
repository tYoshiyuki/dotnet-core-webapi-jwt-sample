using DotNetCoreWebApiJwtSample.Configs;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DotNetCoreWebApiJwtSample.Services
{
    public interface IJwtService
    {
        string GenerateEncodedToken(string userName, IList<string> roles = null);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtConfigurableOptions _jwtConfigurableOptions;
        public JwtService(JwtConfigurableOptions jwtConfigurableOptions)
        {
            _jwtConfigurableOptions = jwtConfigurableOptions;
        }

        /// <summary>
        /// Take user name, device, roles and generate encoded token.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <returns>token string</returns>
        public string GenerateEncodedToken(string userName, IList<string> roles = null)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.Ticks.ToString(), ClaimValueTypes.Integer64),
            };

            if (roles?.Any() == true)
            {
                foreach (string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtConfigurableOptions.JwtIssuer,
                audience: _jwtConfigurableOptions.JwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtConfigurableOptions.JwtExpireDays),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurableOptions.JwtKey)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
