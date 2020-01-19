using System;
using Newtonsoft.Json;

namespace FreshbooksApiClient.Contracts
{
    public class TimeEntry
    {
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("duration")]
        public int? Duration{ get; set; }
        [JsonProperty("project_id")]
        public int? ProjectId{ get; set; }
        [JsonProperty("client_id")]
        public long? ClientId{ get; set; }
        [JsonProperty("is_logged")]
        public bool? IsLogged{ get; set; }
        [JsonProperty("started_at")]
        public DateTime StartedAt{ get; set; }
        [JsonProperty("active")]
        public bool? IsActive{ get; set; }
        [JsonProperty("id")]
        public int? Id{ get; set; }
    }
}