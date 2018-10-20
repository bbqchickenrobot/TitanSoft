using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TitanSoft.Models;

namespace TitanSoft.Api.Extensions
{
    public static class MemberModelAuthExtensions
    {
        public static AuthedUserModel GetAutheneticatedUser(this MemberModel user, string tokenKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "Everyone",
                Issuer = "TitanSoft.com",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthedUserModel()
            {
                Id = user.Id,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
