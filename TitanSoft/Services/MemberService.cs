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
using TitanSoft.Models;
using TitanSoft.Helpers;

namespace TitanSoft.Services
{

    public class MemberService : IUserService, IUserServiceAsync
    {
        private readonly AppSettings _appSettings;
        protected readonly IAsyncDocumentSession db;
        protected readonly UserManager<MemberModel> umanager;
        protected readonly ILogger log;

        public MemberService(IOptions<AppSettings> appSettings, IAsyncDocumentSession db, UserManager<MemberModel> manager, ILogger logger)
        {
            _appSettings = appSettings.Value;
            this.db = db;
            umanager = manager;
            log = logger;
        }

        public async Task<MemberModel> RegisterAsync(RegistrationModel model)
        {
            log.LogInformation($"registering user {model.Email}");
            var user = new MemberModel()
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

        public async Task<MemberModel> AuthenticateAsync(string username, string password)
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
                Audience = "Everyone",
                Issuer = "TitanSoft.com", 
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.UserName),
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

        public async Task<IEnumerable<MemberModel>> GetAllAsync()
        {
            // return users without passwords
            var users = await db.Query<MemberModel>().ToListAsync();
            return  users.Select(x => {
                x.PasswordHash = null;
                return x;
            });
        }

        public async Task DeleteAsync(string id ) =>
            await umanager.DeleteAsync(await umanager.FindByIdAsync(id));

        public void Delete(string id) => DeleteAsync(id).GetAwaiter().GetResult();

        public void Update(MemberModel user) => UpdateAsync(user).GetAwaiter().GetResult();

        public async Task UpdateAsync(MemberModel user) => await umanager.UpdateAsync(user);

        public async Task<MemberModel> GetAsync(string id) => await db.LoadAsync<MemberModel>(id);

        public MemberModel Authenticate(string username, string password) => 
            AuthenticateAsync(username, password).GetAwaiter().GetResult();

        public IEnumerable<MemberModel> GetAll() => GetAllAsync().GetAwaiter().GetResult();

        public MemberModel Register(RegistrationModel model) =>
            RegisterAsync(model).GetAwaiter().GetResult();

        public MemberModel Get(string id) => GetAsync(id).GetAwaiter().GetResult();
    }
}