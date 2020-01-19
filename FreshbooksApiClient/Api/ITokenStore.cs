using System.Threading.Tasks;
using FreshbooksApiClient.Contracts;

namespace FreshbooksApiClient.Api
{
    public interface ITokenStore<T>
    {
        Task StoreToken(T response);
        Task<T> GetTokenAsync();
    }
}