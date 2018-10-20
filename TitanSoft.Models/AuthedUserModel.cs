using System;
using Newtonsoft.Json;

namespace TitanSoft.Models
{
    public class AuthedUserModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
