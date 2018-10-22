using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.Services
{
    public interface IMemberService
    {
        AuthedUserModel Authenticate(string username, string password, string secret);
        List<UserViewModel> GetAll();
        void Register(RegistrationModel model);
        UserViewModel Get(string id);
        void Update(UserViewModel user);
        void Delete(string id);
        void Delete(UserViewModel user);

        Task<AuthedUserModel> AuthenticateAsync(string username, string password, string secret);
        Task<List<UserViewModel>> GetAllAsync();
        Task RegisterAsync(RegistrationModel model);
        Task<UserViewModel> GetAsync(string id);
        Task UpdateAsync(UserViewModel user);
        Task DeleteAsync(string id);
        Task DeleteAsync(UserViewModel user);
    }
}