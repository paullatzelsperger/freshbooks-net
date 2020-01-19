using Newtonsoft.Json;

namespace FreshbooksApiClient.Contracts
{
    public class Metadata
    {
        [JsonProperty("pages")]
        public int? Pages{ get; set; }
        [JsonProperty("total_logged")]
        public int? TotalLogged{ get; set; }
        [JsonProperty("total_unbilled")]
        public int? TotalUnbilled{ get; set; }
        [JsonProperty("per_page")]
        public int? PerPage{ get; set; }
        [JsonProperty("total")]
        public int? Total{ get; set; }
        [JsonProperty("page")]
        public int? Page{ get; set; }

        public bool HasNext()
        {
            return Page < Pages;
        }
    }
}