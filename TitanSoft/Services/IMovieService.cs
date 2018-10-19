using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.Api.Services
{
    public interface IMovieService
    {
        Task SaveAsync(MovieModel movie);
        Task<MovieModel> GetAsync(string id);
        Task<List<MovieModel>> GetAllAsync();
        Task<List<MovieModel>> SearchAsync(string term);
        Task DeleteAsync(string id);
        Task DeleteAsync(MovieModel movie);
        Task UpdateAsync(MovieModel movie);
    }
}
