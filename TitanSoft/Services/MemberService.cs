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
using TitanSoft.Api.Models;
using TitanSoft.Entities;
using TitanSoft.Helpers;

namespace TitanSoft.Services
{

    public class MemberService : IUserService, IUserServiceAsync
    {
        private readonly AppSettings _appSettings;
        protected readonly IAsyncDocumentSession db;
        protected readonly UserManager<AppUser> umanager;
        protected readonly ILogger log;

        public MemberService(IOptions<AppSettings> appSettings, IAsyncDocumentSession db, UserManager<AppUser> manager, ILogger logger)
        {
            _appSettings = appSettings.Value;
            this.db = db;
            umanager = manager;
            log = logger;
        }

        public async Task<AppUser> RegisterAsync(RegistrationModel model)
        {
            log.LogInformation($"registering user {model.Email}");
            var user = new AppUser()
            {
                Email = model.Email,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Id = model.Email,
                UserName = model.Email
            };

            await umanager.AddPasswordAsync(user, model.Password);
            var result =  await umanager.CreateAsync(user);
            await db.SaveChangesAsync();
            log.LogInformation($"successfully registered user");
            return user;
        }

        public async Task<AppUser> AuthenticateAsync(string username, string password)
        {
            log.LogInformation($"authenticating user {username}");
            var user = await umanager.FindByIdAsync(username);

            if(user == null){
                log.LogInformation($"unable to find user");
                return null;
            }

            var result = await umanager.CheckPasswordAsync(user, password);
            if (!result)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("id", user.Id)
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

        public async Task DeleteAsync(string id ) =>
            await umanager.DeleteAsync(await umanager.FindByIdAsync(id));

        public void Delete(string id) => DeleteAsync(id).GetAwaiter().GetResult();

        public void Update(AppUser user) => UpdateAsync(user).GetAwaiter().GetResult();

        public async Task UpdateAsync(AppUser user) => await umanager.UpdateAsync(user);

        public async Task<AppUser> GetAsync(string id) => await db.LoadAsync<AppUser>(id);

        public AppUser Authenticate(string username, string password) => 
            AuthenticateAsync(username, password).GetAwaiter().GetResult();

        public IEnumerable<AppUser> GetAll() => GetAllAsync().GetAwaiter().GetResult();

        public AppUser Register(RegistrationModel model) =>
            RegisterAsync(model).GetAwaiter().GetResult();

        public AppUser Get(string id) => GetAsync(id).GetAwaiter().GetResult();
    }
}