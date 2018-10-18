using System;
using Newtonsoft.Json;

namespace TitanSoft.Models
{
    public class RentalModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("movie_id")]
        public string MovieId { get; set; }

        [JsonProperty("date")]
        public DateTime? DateRented { get; set; } = DateTime.Now;

        [JsonProperty("price")]
        public Decimal Price { get; set; }

        [JsonProperty("expiring")]
        public DateTime? Expiring { get => DateRented?.AddDays(14); }
    }
}
