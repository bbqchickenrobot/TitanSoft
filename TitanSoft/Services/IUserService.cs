using System.Collections.Generic;
using TitanSoft.Entities;

namespace TitanSoft.Services
{
    public interface IUserService
    {
        AppUser Authenticate(string username, string password);
        IEnumerable<AppUser> GetAll();
    }
}