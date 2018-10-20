using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TitanSoft.Models
{
    public class OmdbSearchModel
    {
        [JsonProperty("Search")]
        public List<OmdbSearchResult> Search { get; set; }
    }
}
