using System;
using System.Collections.Generic;
using TitanSoft.Models;

namespace TitanSoft.DataAccess
{
    public class OmdbApi : IOmdbApi
    {
        public OmdbApi()
        {
        }

        public MovieModel GetMovie(string id)
        {
            throw new NotImplementedException();
        }

        public List<MovieModel> Search(string term)
        {
            throw new NotImplementedException();
        }
    }
}
