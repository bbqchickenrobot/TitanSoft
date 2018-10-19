using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using TitanSoft.DataAccess;
using TitanSoft.Models;

namespace TitanSoft.Api.Services
{
    public class MovieService : IMovieService
    {
        readonly IAsyncDocumentSession db;
        readonly ILogger log;

        public MovieService(IAsyncDocumentSession session, ILogger logger)
        {
            db = session;
            log = logger;
        }

        public async Task DeleteAsync(string id) => await Task.Run(() => db.Delete(id));

        public async Task DeleteAsync(MovieModel movie) => await Task.Run(() => db.Delete(movie));

        public async Task<MovieModel> GetAsync(string id) => await db.LoadAsync<MovieModel>(id);

        public async Task<List<MovieModel>> GetAllAsync() => await db.Query<MovieModel>().ToListAsync();

        public async Task SaveAsync(MovieModel movie)
        {
            await db.StoreAsync(movie);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(MovieModel movie)
        {
            await db.StoreAsync(movie);
            await db.SaveChangesAsync();
        }

        public async Task<List<MovieModel>> SearchAsync(string term)
        {
            var results = await db.Query<MovieModel>()
                                  .Search(x => x.Actors, term)
                                  .Search(x => x.Plot, term)
                                  .Search(x => x.Genre, term)
                                  .Search(x => x.Title, term)
                                  .ToListAsync();
            return results;
        }
    }
}
