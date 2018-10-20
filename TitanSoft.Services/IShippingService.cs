using TitanSoft.Models;

namespace TitanSoft.Services
{
    public interface IShippingService
    {
        void Ship(MemberModel member, MovieModel movie);
    }
}
