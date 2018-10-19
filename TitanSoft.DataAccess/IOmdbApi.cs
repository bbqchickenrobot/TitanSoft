using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.DataAccess
{
    public interface IOmdbApi{
        MovieModel GetMovie(string id);
        SearchModel Search(string term, int? page);
        Task<MovieModel> GetMovieAsync(string id);
        Task<SearchModel> SearchAsync(string term, int? page);
    }
}
