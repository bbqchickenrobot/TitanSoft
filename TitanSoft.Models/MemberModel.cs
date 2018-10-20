using System;
using Newtonsoft.Json;
using Raven.Identity;
using TitanSoft.DataAccess;

namespace TitanSoft.Models
{
    public class MemberModel : IdentityUser, IPersistable
    {
        [JsonProperty("firstname")]
        public string Firstame { get; set; }
        [JsonProperty("lastname")]
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
    }
}
