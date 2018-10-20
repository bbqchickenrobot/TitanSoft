using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.Services
{
    public interface IMemberService
    {
        MemberModel Authenticate(string username, string password);
        List<MemberModel> GetAll();
        MemberModel Register(RegistrationModel model);
        MemberModel Get(string id);
        void Update(MemberModel user);
        void Delete(string id);

        Task<MemberModel> AuthenticateAsync(string username, string password);
        Task<List<MemberModel>> GetAllAsync();
        Task<MemberModel> RegisterAsync(RegistrationModel model);
        Task<MemberModel> GetAsync(string id);
        Task UpdateAsync(MemberModel user);
        Task DeleteAsync(string id);
    }
}