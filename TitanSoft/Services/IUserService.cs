using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Api.Models;
using TitanSoft.Models;

namespace TitanSoft.Services
{
    public interface IUserService : IUserServiceAsync
    {
        MemberModel Authenticate(string username, string password);
        IEnumerable<MemberModel> GetAll();
        MemberModel Register(RegistrationModel model);
        MemberModel Get(string id);
        void Update(MemberModel user);
        void Delete(string id);
    }


    public interface IUserServiceAsync{
        Task<MemberModel> AuthenticateAsync(string username, string password);
        Task<IEnumerable<MemberModel>> GetAllAsync();
        Task<MemberModel> RegisterAsync(RegistrationModel model);
        Task<MemberModel> GetAsync(string id);
        Task UpdateAsync(MemberModel user);
        Task DeleteAsync(string id);
    }
}