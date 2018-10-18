using System;
using Newtonsoft.Json;

namespace TitanSoft.Api.Models
{
    [JsonObject()]
    public class RegistrationModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstname")]
        public string Firstname { get; set; }
        [JsonProperty("lastname")]
        public string Lastname { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
