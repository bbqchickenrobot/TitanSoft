using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Api.Models;
using TitanSoft.Entities;

namespace TitanSoft.Services
{
    public interface IUserService : IUserServiceAsync
    {
        AppUser Authenticate(string username, string password);
        IEnumerable<AppUser> GetAll();
        AppUser Register(RegistrationModel model);
        AppUser Get(string id);
        void Update(AppUser user);
        void Delete(string id);
    }


    public interface IUserServiceAsync{
        Task<AppUser> AuthenticateAsync(string username, string password);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<AppUser> RegisterAsync(RegistrationModel model);
        Task<AppUser> GetAsync(string id);
        Task UpdateAsync(AppUser user);
        Task DeleteAsync(string id);
    }
}