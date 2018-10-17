using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.DataAccess
{
    public interface IOmdbApiAsync
    {
        Task<MovieModel> GetMovieAsync(string id);
        Task<SearchModel> SearchAsync(string term);
    }

    public interface IOmdbApi{
        MovieModel GetMovie(string id);
        SearchModel Search(string term);
    }
}
