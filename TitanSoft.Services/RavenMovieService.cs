using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using TitanSoft.DataAccess;
using TitanSoft.Models;

namespace TitanSoft.Services
{
    public sealed class RavenMovieService : IMovieService
    {
        readonly IAsyncDocumentSession db;
        readonly ILogger log;

        public RavenMovieService(IAsyncDocumentSession session, ILogger logger)
        {
            db = session;
            log = logger;
        }

        public void Delete(MovieModel movie) => 
            new RavenDeleteEntityCommand<MovieModel>(db, log).Delete(movie);

        // RavenDB does not have an async Delete() method hence the Task.Run()
        public Task DeleteAsync(MovieModel movie) => Task.Run(() => Delete(movie));

        public async Task<MovieModel> GetAsync(string id) => await db.LoadAsync<MovieModel>(id);

        public async Task<List<MovieModel>> GetAllAsync() => 
            await new RavenGetAllEntitiesQuery<MovieModel>(db, log).GetAllAsync();

        public async Task SaveAsync(MovieModel movie) =>
            await new RavenSaveEntityCommand<MovieModel>(db, log).StoreAsync(movie);

        public async Task UpdateAsync(MovieModel movie) =>
            await new RavenSaveEntityCommand<MovieModel>(db, log).UpdateAsync(movie);

        public async Task<List<MovieModel>> SearchAsync(string term) => 
            await new RavenMovieSearchQuery(db, log).SearchAsync(term);
        
        public void Save(MovieModel movie) => SaveAsync(movie).GetAwaiter().GetResult();

        public MovieModel Get(string id) => GetAsync(id).GetAwaiter().GetResult();

        public List<MovieModel> GetAll() => GetAllAsync().GetAwaiter().GetResult();

        public List<MovieModel> Search(string term) => SearchAsync(term).GetAwaiter().GetResult();

        public void Update(MovieModel movie) => UpdateAsync(movie).GetAwaiter().GetResult();

        public async Task<List<MovieModel>> PagedSearchAsync(string term, int page) =>
            await new RavenPagedMovieSearchQuery(db, log).SearchAsync(term, page);

        public List<MovieModel> PagedSearch(string term, int page) => 
                PagedSearchAsync(term, page).GetAwaiter().GetResult();
    }
}
