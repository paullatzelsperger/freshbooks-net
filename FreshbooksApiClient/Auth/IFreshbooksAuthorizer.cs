using System.Threading.Tasks;
using FreshbooksApiClient.Contracts;

namespace FreshbooksApiClient.Auth
{
    public interface IFreshbooksAuthorizer
    {
        Task<string> StartAuthorization(string startUrl, string redirectUri, string clientId);

        Task<FreshbooksTokens> RequestTokens(string apiBaseUrl, string authCode, string clientId, string clientSecret,
            string redirectUri);

        Task<FreshbooksTokens> RefreshTokenRequest(string apiBaseUrl,
            string refreshToken,
            string clientId, string clientSecret, object redirectUri);
    }
}