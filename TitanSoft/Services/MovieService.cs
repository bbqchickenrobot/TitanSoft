using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using TitanSoft.DataAccess;
using TitanSoft.Models;

namespace TitanSoft.Api.Services
{
    public class MovieService : IMovieService
    {
        readonly IAsyncDocumentSession db;
        readonly ILogger log;
        IConfiguration config;

        public MovieService(IAsyncDocumentSession session, ILogger logger, IConfiguration configuration)
        {
            db = session;
            log = logger;
            config = configuration;
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

        public async Task<List<Search>> SearchAsync(string term)
        {
            var api = new OmdbApi(config, log);
            var results = await api.SearchAsync(term);
            return results.Search;
        }
    }
}
