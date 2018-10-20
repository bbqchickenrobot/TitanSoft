using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace TitanSoft.DataAccess
{
    public sealed class RavenGetAllEntitiesQuery<T> : IGetAllQuery<T> where T : IPersistable
    {
        readonly IAsyncDocumentSession db;
        readonly ILogger logger;

        public RavenGetAllEntitiesQuery(IAsyncDocumentSession session, ILogger logger)
        {
            this.logger = logger;
            this.db = session;
        }

        public List<T> GetAll() => GetAllAsync().GetAwaiter().GetResult();

        public async Task<List<T>> GetAllAsync() => await db.Query<T>().ToListAsync();

    }
}
