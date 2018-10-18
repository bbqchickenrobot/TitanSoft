using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.Api.Services
{
    public interface IRentalService {
        Task RentAsync(RentalModel model);
        Task<List<RentalModel>> GetHistory(string userId);
    }
}
