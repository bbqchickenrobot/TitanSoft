using TitanSoft.Models;

namespace TitanSoft.Api.Services
{
    public interface IShippingService
    {
        void Ship(MemberModel member);
    }

    public class FauxShippingService : IShippingService
    {
        public void Ship(MemberModel member)
        {
        }
    }
}
