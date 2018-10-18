using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.Entities;
using TitanSoft.Helpers;

namespace TitanSoft.Services
{

    public class UserService : IUserService, IUserServiceAsync
    {
        private readonly AppSettings _appSettings;
        protected readonly IAsyncDocumentSession db;
        protected readonly UserManager<AppUser> umanager;
        protected readonly ILogger log;

        public UserService(IOptions<AppSettings> appSettings, IAsyncDocumentSession db, UserManager<AppUser> manager, ILogger logger)
        {
            _appSettings = appSettings.Value;
            this.db = db;
            umanager = manager;
            log = logger;
        }

        public async Task<AppUser> RegisterAsync(string email, string firstname, string lastname, string password){
            log.LogInformation($"registering user {email}");
            var user = new AppUser()
            {
                Email = email,
                FirstName = firstname,
                LastName = lastname
            };

            await umanager.AddPasswordAsync(user, password);
            var result =  await umanager.CreateAsync(user);
            log.LogInformation($"successfully registered user");
            return user;
        }

        public async Task<AppUser> AuthenticateAsync(string username, string password)
        {
            log.LogInformation($"authenticating user {username}");
            var user = await db.Query<AppUser>().SingleOrDefaultAsync(x => x.UserName == username || x.Email == username);

            if(user == null){
                log.LogInformation($"unable to find user");
                return null;
            }
            var hash = umanager.PasswordHasher.HashPassword(user, password);
            var result = umanager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            

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
            log.LogDebug($"user token: {user.Token}");
            // remove password before returning
            user.PasswordHash = null;
            log.LogInformation($"successfully logged in user {username}");
            return user;
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            // return users without passwords
            var users = await db.Query<AppUser>().ToListAsync();
            return  users.Select(x => {
                x.PasswordHash = null;
                return x;
            });
        }

        public void Update(AppUser user) => UpdateAsync(user).GetAwaiter().GetResult();

        public async Task UpdateAsync(AppUser user)
        {
            var result = await umanager.UpdateAsync(user);
        }

        public async Task<AppUser> GetAsync(string id) => await db.LoadAsync<AppUser>(id);

        public AppUser Authenticate(string username, string password) => 
            AuthenticateAsync(username, password).GetAwaiter().GetResult();

        public IEnumerable<AppUser> GetAll() => GetAllAsync().GetAwaiter().GetResult();

        public AppUser Register(string email, string firstname, string lastname, string password) =>
            RegisterAsync(email, firstname, lastname, password).GetAwaiter().GetResult();

        public AppUser Get(string id) => GetAsync(id).GetAwaiter().GetResult();
    }
}