using System;
using Newtonsoft.Json;

namespace TitanSoft.Models
{
    [JsonObject()]
    public class UserViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get => Id; }

        [JsonProperty("firstname")]
        public string Firstname {get; set;}

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("email")]
        public string Email { get => Id; }

        public string Lastame { get; set; }
        [JsonProperty("address1")]

        public string Address1 { get; set; }
        [JsonProperty("city")]

        public string City { get; set; }
        [JsonProperty("state")]

        public string State { get; set; }

        [JsonProperty("postalcode")]
        public string PostalCode { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        [JsonProperty("aboutme")]
        public string AboutMe { get; set; }

        [JsonProperty("password")]
        public string password { get; set;}

        [JsonProperty("phonenumber")]
        public string phonenumber { get; set; }
    }
}

