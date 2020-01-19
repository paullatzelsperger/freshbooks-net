using System.Collections.Generic;
using Newtonsoft.Json;

namespace FreshbooksApiClient.Contracts
{
    public class TimeEntryResponse
    {
        [JsonProperty("time_entries")]
        public IEnumerable<TimeEntry> TimeEntries { get; set; }
        
        [JsonProperty("meta")]
        public Metadata Meta { get; set; }
    }
}