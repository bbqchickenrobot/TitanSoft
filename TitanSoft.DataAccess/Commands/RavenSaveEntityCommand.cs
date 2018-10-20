using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;

namespace TitanSoft.DataAccess
{
    /// <summary>
    /// Command Object that persists data in RavenDB
    /// </summary>
    public sealed class RavenSaveEntityCommand<T> : IRavenPersistenceCommand<T> where T : IPersistable
    {
        readonly IAsyncDocumentSession session;
        readonly ILogger log;

        public RavenSaveEntityCommand(IAsyncDocumentSession session, ILogger logger)
        {
            log = logger;
            this.session = session;
        }

        public void Store(T param) => StoreAsync(param).GetAwaiter().GetResult();

        public async Task StoreAsync(T param)
        {
            await session.StoreAsync(param);
            await session.SaveChangesAsync();
        }

        public void Update(T param) => UpdateAsync(param).GetAwaiter().GetResult();

        public async Task UpdateAsync(T param)
        {
            await session.StoreAsync(param, param.Id);
            await session.SaveChangesAsync();
        }
    }
}
