using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;

namespace TitanSoft.DataAccess
{
    public sealed class RavenDeleteEntityCommand<T> : IDeleteCommand<T> where T : IPersistable
    {
        readonly IAsyncDocumentSession db;
        readonly ILogger logger;

        public RavenDeleteEntityCommand(IAsyncDocumentSession session, ILogger logger)
        {
            this.logger = logger;
            this.db = session;
        }

        public void Delete(T param) => db.Delete(param);

        public Task DeleteAsync(T param) => Task.Run(() => Delete(param));
    }
}