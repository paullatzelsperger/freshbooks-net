using System;
using System.Net.Mime;
using System.Security.Authentication;
using System.Threading.Tasks;
using FreshbooksApiClient.Contracts;
using FreshbooksApiClient.Rest;
using IdentityModel.OidcClient.Browser;
using RedirectServer.Browser;
using RestSharp;
using BrowserOptions = RedirectServer.Browser.BrowserOptions;

namespace FreshbooksApiClient.Auth
{
    public class FreshbooksAuthorizerImpl : IFreshbooksAuthorizer
    {
        private readonly SystemBrowser _browser;
        private readonly IRestClientFactory _factory;

        public FreshbooksAuthorizerImpl(SystemBrowser browser, IRestClientFactory factory)
        {
            _browser = browser;
            _factory = factory;
        }

        public async Task<string> StartAuthorization(string startUrl, string redirectUri, string clientId)
        {
            if (startUrl == null) throw new ArgumentNullException(nameof(startUrl));
            if (redirectUri == null) throw new ArgumentNullException(nameof(redirectUri));
            if (clientId == null) throw new ArgumentNullException(nameof(clientId));

            redirectUri = redirectUri.Replace("http://", "https://");
            startUrl = $"{startUrl}?client_id={clientId}&response_type=code&redirect_uri={redirectUri}";

            var port = ParsePort(redirectUri);
            _browser.Port = port;
            var browserResult = await _browser.InvokeAsync(new BrowserOptions
            {
                StartUrl = startUrl
            });

            if (browserResult.ResultType != BrowserResultType.Success)
                throw new AuthenticationException(browserResult.Error);

            var response = browserResult.Response;
            return ExtractCode(response);
        }

        public async Task<FreshbooksTokens> RequestTokens(string apiBaseUrl, string authCode, string clientId,
            string clientSecret,
            string redirectUri)
        {
            var url = $"{apiBaseUrl}/auth/oauth/token";
            var request = new RestRequest(url, Method.POST);

            request.AddHeader("Api-Version", "alpha")
                .AddHeader("Content-Type", MediaTypeNames.Application.Json);
            // .AddParameter("grant_type", "authorization_code")
            // .AddParameter("client_secret", clientSecret)
            // .AddParameter("client_id", clientId)
            // .AddParameter("code", authCode)
            // .AddParameter("redirect_uri", redirectUri);
            var payload = new
            {
                grant_type = "authorization_code",
                client_secret = clientSecret,
                client_id = clientId,
                code = authCode,
                redirect_uri = redirectUri
            };
            request.AddJsonBody(payload);

            var client = await _factory.CreateRestClient();

            var response = await client.ExecutePostAsync<FreshbooksTokens>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }

            throw new AuthenticationException(
                $"code={response.StatusCode.ToString()} message={response.Content}, error={response.ErrorMessage}");
        }


        private string ExtractCode(string response)
        {
            var rawItems = response.Split("&");
            var codeExpr = Array.Find(rawItems, s => s.Contains("code="));
            return codeExpr != null ? codeExpr.Substring(codeExpr.IndexOf("=", StringComparison.Ordinal) + 1) : "";
        }

        private int ParsePort(string urlString)
        {
            var url = new Uri(urlString);
            return url.Port;
        }
    }
}