using System;
using Newtonsoft.Json;

namespace TitanSoft.Models
{
    [JsonObject()]
    public class LoginModel
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
