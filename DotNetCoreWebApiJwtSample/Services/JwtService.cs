using DotNetCoreWebApiJwtSample.Configs;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DotNetCoreWebApiJwtSample.Services.Interfaces;

namespace DotNetCoreWebApiJwtSample.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfigurableOptions _jwtConfigurableOptions;

        public JwtService(JwtConfigurableOptions jwtConfigurableOptions)
        {
            _jwtConfigurableOptions = jwtConfigurableOptions;
        }

        /// <summary>
        /// JWTトークンを生成します
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public string GenerateEncodedToken(string userName, IList<string> roles = null)
        {
            // claimを構築する
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.Ticks.ToString(CultureInfo.CurrentCulture), ClaimValueTypes.Integer64),
            };

            if (roles?.Any() == true)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            // JWTトークンを生成する
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
