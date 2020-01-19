using System;
using Newtonsoft.Json;

namespace FreshbooksApiClient.Contracts
{
    public class TimeEntryFilter
    {
        [JsonProperty("billable")] public bool Billable { get; set; } = true;

        [JsonProperty("billed")] public bool Billed { get; set; } = false;

        [JsonProperty("client_id")] public int ClientId { get; set; }
        [JsonProperty("include_deleted")] public bool IncludeDeleted { get; set; } = false;
        [JsonProperty("team")] public bool EntireTeam { get; set; } = true;
        [JsonProperty("include_unlogged")] public bool IncludeRunningTimers { get; set; } = false;
        [JsonProperty("started_from")] public DateTime StartDateUtc { get; set; }
        [JsonProperty("started_to")] public DateTime EndDateUtc { get; set; }
        [JsonProperty("updated_since")] public string UpdatedSinceUtc { get; set; }

        public static readonly TimeEntryFilter CurrentMonth = new TimeEntryFilter
        {
            StartDateUtc = FirstDayOfMonth(DateTime.Now),
            EndDateUtc = LastDayOfMonth(DateTime.Now)
        };

        private static DateTime LastDayOfMonth(in DateTime now)
        {
            return new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
        }


        private static DateTime FirstDayOfMonth(in DateTime now)
        {
            return new DateTime(now.Year, now.Month, 1);
        }
    }
}