using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.Entities;
using TitanSoft.Helpers;

namespace TitanSoft.Services
{

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        protected readonly IDocumentStore store;
        protected readonly IAsyncDocumentSession db;

        public UserService(IOptions<AppSettings> appSettings, IDocumentStore store)
        {
            _appSettings = appSettings.Value;
            this.store = store;
            this.db = store.OpenAsyncSession("TitanSoft");
        }

        public AppUser Authenticate(string username, string password)
        {
            var user = db.Query<AppUser>().SingleOrDefault(x => x.UserName == username && x.PasswordHash == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.PasswordHash = null;

            return user;
        }

        public IEnumerable<AppUser> GetAll()
        {
            // return users without passwords
            return db.Query<AppUser>().ToList().Select(x => {
                x.PasswordHash = null;
                return x;
            });
        }
    }
}