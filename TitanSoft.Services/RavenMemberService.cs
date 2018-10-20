using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.DataAccess;
using TitanSoft.Models;

namespace TitanSoft.Services
{

    public class RavenMemberService : IMemberService
    {
        protected readonly IAsyncDocumentSession db;
        protected readonly UserManager<MemberModel> umanager;
        protected readonly ILogger log;

        public RavenMemberService(IAsyncDocumentSession db, UserManager<MemberModel> manager, ILogger logger)
        {
            this.db = db;
            umanager = manager;
            log = logger;
        }

        public async Task<MemberModel> RegisterAsync(RegistrationModel model)
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
                
            // remove password before returning
            user.PasswordHash = null;
            log.LogInformation($"successfully logged in user {username}");
            return user;
        }

        public async Task<List<MemberModel>> GetAllAsync()
        {
            // return users without passwords
            var users = (await new RavenGetAllEntitiesQuery<MemberModel>(db, log).GetAllAsync())
                .Select(x =>
                {
                    x.PasswordHash = "";
                    return x;
                });
            return users.ToList();
        }

        public async Task DeleteAsync(string id ) =>
            await umanager.DeleteAsync(await umanager.FindByIdAsync(id));

        public void Delete(string id) => DeleteAsync(id).GetAwaiter().GetResult();

        public void Update(MemberModel user) => UpdateAsync(user).GetAwaiter().GetResult();

        public async Task UpdateAsync(MemberModel user) => await umanager.UpdateAsync(user);

        public async Task<MemberModel> GetAsync(string id) => 
            await new RavenFindEntityByIdQuery<MemberModel>(db, log).FindByIdAsync(id);

        public MemberModel Authenticate(string username, string password) => 
            AuthenticateAsync(username, password).GetAwaiter().GetResult();

        public List<MemberModel> GetAll() => GetAllAsync().GetAwaiter().GetResult();

        public MemberModel Register(RegistrationModel model) =>
            RegisterAsync(model).GetAwaiter().GetResult();

        public MemberModel Get(string id) => GetAsync(id).GetAwaiter().GetResult();
    }
}
