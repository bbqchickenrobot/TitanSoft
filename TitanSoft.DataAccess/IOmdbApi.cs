using System;
using System.Collections.Generic;
using TitanSoft.Models;

namespace TitanSoft.DataAccess
{
    public interface IOmdbApi
    {
         MovieModel GetMovie(string id);
         List<MovieModel> Search(string term);
    }
}
