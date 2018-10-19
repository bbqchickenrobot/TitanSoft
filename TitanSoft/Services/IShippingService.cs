using TitanSoft.Models;

namespace TitanSoft.Api.Services
{
    public interface IShippingService
    {
        void Ship(MemberModel member, MovieModel movie);
    }
}
