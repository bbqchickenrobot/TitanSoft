using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.Services
{
    public interface IMovieService
    {
        void Save(MovieModel movie);
        MovieModel Get(string id);
        List<MovieModel> GetAll();
        List<MovieModel> Search(string term);
        void Delete(MovieModel movie);
        void Update(MovieModel movie);
        List<MovieModel> PagedSearch(string term, int page);

        Task SaveAsync(MovieModel movie);
        Task<MovieModel> GetAsync(string id);
        Task<List<MovieModel>> GetAllAsync();
        Task<List<MovieModel>> SearchAsync(string term);
        Task DeleteAsync(MovieModel movie);
        Task UpdateAsync(MovieModel movie);
        Task<List<MovieModel>> PagedSearchAsync(string term, int page);
    }
}
