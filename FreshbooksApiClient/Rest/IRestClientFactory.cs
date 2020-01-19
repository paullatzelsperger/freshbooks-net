using System.Threading.Tasks;
using RestSharp;

namespace FreshbooksApiClient.Rest
{
    public interface IRestClientFactory
    {
        Task<IRestClient> CreateRestClient();

    }
}