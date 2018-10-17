using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TitanSoft.Models
{
    public class SearchModel
    {
        [JsonProperty("Search")]
        public List<Search> Search { get; set; }
    }

    public class Search
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Year")]
        public long Year { get; set; }

        [JsonProperty("imdbID")]
        public string ImdbId { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Poster")]
        public Uri Poster { get; set; }
    }
}
