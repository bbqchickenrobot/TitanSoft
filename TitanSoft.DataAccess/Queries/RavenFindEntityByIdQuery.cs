using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;

namespace TitanSoft.DataAccess
{
    public sealed class RavenFindEntityByIdQuery<T> : IFindByIdQuery<T, string>
            where T : IPersistable
    {
        readonly IAsyncDocumentSession db;
        readonly ILogger logger;

        public RavenFindEntityByIdQuery(IAsyncDocumentSession session, ILogger logger)
        {
            this.logger = logger;
            this.db = session;
        }

        public T FindById(string id) => FindByIdAsync(id).GetAwaiter().GetResult();

        public async Task<T> FindByIdAsync(string id) => await db.LoadAsync<T>(id);
    }
}