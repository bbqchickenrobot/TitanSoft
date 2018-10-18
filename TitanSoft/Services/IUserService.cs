using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Entities;

namespace TitanSoft.Services
{
    public interface IUserService : IUserServiceAsync
    {
        AppUser Authenticate(string username, string password);
        IEnumerable<AppUser> GetAll();
        AppUser Register(string email, string firstname, string lastname, string password);
        AppUser Get(string id);
        void Update(AppUser user);
    }


    public interface IUserServiceAsync{
        Task<AppUser> AuthenticateAsync(string username, string password);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<AppUser> RegisterAsync(string email, string firstname, string lastname, string password);
        Task<AppUser> GetAsync(string id);
        Task UpdateAsync(AppUser user);
    }
}