using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TitanSoft.Models;

namespace TitanSoft.DataAccess
{
    public interface IOmdbApi{
        MovieModel GetMovie(string id);
        OmdbSearchModel Search(string term, int? page);
        Task<MovieModel> GetMovieAsync(string id);
        Task<OmdbSearchModel> SearchAsync(string term, int? page);
    }
}
