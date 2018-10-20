using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.Models;

namespace TitanSoft.DataAccess
{
    public sealed class RavenMovieSearchQuery : ISearchQuery<MovieModel>
    {
        readonly IAsyncDocumentSession db;
        readonly ILogger log;

        public RavenMovieSearchQuery(IAsyncDocumentSession session, ILogger log)
        {
            this.log = log;
            this.db = session;
        }

        public async Task<List<MovieModel>> SearchAsync(string term) =>
                         await db.Query<MovieModel>()
                                  .Search(x => x.Actors, term)
                                  .Search(x => x.Plot, term)
                                  .Search(x => x.Genre, term)
                                  .Search(x => x.Title, term)
                                  .ToListAsync();
    }
}
