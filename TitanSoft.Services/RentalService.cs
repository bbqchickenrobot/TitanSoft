using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.Models;

namespace TitanSoft.Services
{
    public class RentalService : IRentalService
    {
        readonly ILogger log;
        readonly IAsyncDocumentSession db;
        private readonly UserManager<MemberModel> userManager;

        public RentalService(IAsyncDocumentSession session, UserManager<MemberModel> userManager, ILogger logger)
        {
            log = logger;
            db = session;
            this.userManager = userManager;
        }

        public async Task<List<RentalModel>> GetHistoryAsync(string userId) =>
            await db.Query<RentalModel>().Where(x => x.UserId == userId).ToListAsync();

        public async Task RentAsync(RentalModel model)
        {
            await db.StoreAsync(model);
            await db.SaveChangesAsync();
        }
    }
}
