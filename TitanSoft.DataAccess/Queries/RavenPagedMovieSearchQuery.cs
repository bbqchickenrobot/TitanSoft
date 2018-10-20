using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.Models;

namespace TitanSoft.DataAccess
{
    public sealed class RavenPagedMovieSearchQuery : IPagedSearchQuery<MovieModel>
    {
        readonly IAsyncDocumentSession db;
        readonly ILogger log;

        public RavenPagedMovieSearchQuery(IAsyncDocumentSession session, ILogger log)
        {
            this.log = log;
            this.db = session;
        }

        public async Task<List<MovieModel>> SearchAsync(string term, int page = 1, int recs = 20)
        {
            Math.Abs(page = page < 2 || page <= 1 ? 0 : page - 1);
            return await db.Query<MovieModel>()
                       .Search(x => x.Actors, term)
                       .Search(x => x.Plot, term)
                       .Search(x => x.Genre, term)
                       .Search(x => x.Title, term)
                       .Skip(page*recs)
                       .Take(recs)
                       .ToListAsync();
        }
    }
}
