using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.Services
{
    public interface IRentalService {
        Task RentAsync(RentalModel model);
        Task<List<RentalModel>> GetHistoryAsync(string userId);
    }
}
