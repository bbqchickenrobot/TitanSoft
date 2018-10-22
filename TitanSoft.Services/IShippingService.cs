using TitanSoft.Models;

namespace TitanSoft.Services
{
    public interface IShippingService
    {
        void Ship(UserViewModel member, MovieModel movie);
    }
}
