using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.DataAccess;
using TitanSoft.Models;

namespace TitanSoft.Services
{

    public class RavenMemberService : IMemberService
    {
        readonly IAsyncDocumentSession db;
        readonly UserManager<MemberModel> umanager;
        readonly ILogger log;
        readonly IConfiguration configuration;

        public RavenMemberService(IAsyncDocumentSession db, UserManager<MemberModel> manager, 
                                  IConfiguration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.db = db;
            umanager = manager;
            log = logger;
        }

        public async Task RegisterAsync(RegistrationModel model)
        {
            log.LogDebug($"registering user {model.Email}");

            var user = new MemberModel()
            {
                Email = model.Email,
                Id = model.Email,
                UserName = model.Email
            };

            await umanager.AddPasswordAsync(user, model.Password);
            var result =  await umanager.CreateAsync(user);
            await db.SaveChangesAsync();
            log.LogInformation($"successfully registered user {model.Email}");
            return;
        }

        public async Task<AuthedUserModel> AuthenticateAsync(string username, string password, string key)
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
                
            log.LogInformation($"successfully logged in user {username}");
            return new AuthedUserModel
            {
                Id = user.Id,
                Name = $"{user.Firstame} {user.Lastame}",
                Token = GenerateToken(user, key)
            };
        }

        protected string GenerateToken(MemberModel user, string tokenKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "Everyone",
                Issuer = "TitanSoft.com",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<List<UserViewModel>> GetAllAsync() =>
            (await new RavenGetAllEntitiesQuery<MemberModel>(db, log).GetAllAsync())
                        .Select(x => x.ToViewModel()).ToList();

        public async Task DeleteAsync(string id ) =>
            await umanager.DeleteAsync(await umanager.FindByIdAsync(id));

        public void Delete(string id) => DeleteAsync(id).GetAwaiter().GetResult();

        public void Update(UserViewModel user) => UpdateAsync(user).GetAwaiter().GetResult();

        public async Task UpdateAsync(UserViewModel model)
        {
            var member = await umanager.FindByIdAsync(model.Id);
            member.UpdateFromViewModel(model);
            var result = await umanager.UpdateAsync(member);
        }

        public void UpdatePassword(MemberModel user, string current, string password) =>
                UpdatePasswordAsync(user, current, password).GetAwaiter().GetResult();

        public async Task UpdatePasswordAsync(MemberModel user, string current, string password)
        {
            await umanager.ChangePasswordAsync(user, current, password);        
        }

        public async Task<UserViewModel> GetAsync(string id) =>
            (await new RavenFindEntityByIdQuery<MemberModel>(db, log).FindByIdAsync(id)).ToViewModel();

        public UserViewModel Get(string id) => GetAsync(id).GetAwaiter().GetResult();

        public List<UserViewModel> GetAll() => GetAllAsync().GetAwaiter().GetResult();

        public AuthedUserModel Authenticate(string username, string password, string secret) =>
                AuthenticateAsync(username, password, secret).GetAwaiter().GetResult();

        public void Register(RegistrationModel model) =>
            RegisterAsync(model).GetAwaiter().GetResult();

        public void Delete(UserViewModel user) => Delete(user.Id);

        public Task DeleteAsync(UserViewModel user) => DeleteAsync(user.Id);
    }
}
