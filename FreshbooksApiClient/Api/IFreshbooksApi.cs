using System.Collections.Generic;
using System.Threading.Tasks;
using FreshbooksApiClient.Contracts;

namespace FreshbooksApiClient.Api
{
    public interface IFreshbooksApi
    {
        Task Authenticate();
        Task<IdentityInfo> GetIdentityInfo();
        Task<TimeEntryResponse> GetTimeEntries(TimeEntryFilter filter, string businessId);
        
    }
}