using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.Models;

namespace TitanSoft.Api.Services
{
    public class RentalService : IRentalService
    {
        readonly ILogger log;
        readonly IAsyncDocumentSession db;

        public RentalService(IAsyncDocumentSession session, ILogger logger)
        {
            log = logger;
            db = session;
        }

        public Task<List<RentalModel>> GetHistory(string userId) =>
            db.Query<RentalModel>().Where(x => x.UserId == userId).ToListAsync();

        public async Task RentAsync(RentalModel model)
        {
            await db.StoreAsync(model);
            await db.SaveChangesAsync();
        }
    }
}
