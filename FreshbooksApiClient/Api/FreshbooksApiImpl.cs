using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Security.Authentication;
using System.Threading.Tasks;
using FreshbooksApiClient.Auth;
using FreshbooksApiClient.Contracts;
using FreshbooksApiClient.Rest;
using FreshbooksTimeEntryGenerator.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace FreshbooksApiClient.Api
{
    public class FreshbooksApiImpl : IFreshbooksApi
    {
        private readonly IFreshbooksAuthorizer _authorizer;
        private readonly ITokenStore<FreshbooksTokens> _tokenStore;
        private readonly FreshbooksAuthSettings _authSettings;
        private readonly FreshbooksAppSettings _appSettings;
        private readonly ILogger<FreshbooksApiImpl> _logger;
        private readonly IRestClientFactory _factory;

        public FreshbooksApiImpl(IFreshbooksAuthorizer authorizer, IOptionsMonitor<FreshbooksAuthSettings> authSettings,
            IOptionsMonitor<FreshbooksAppSettings> appSettings, ITokenStore<FreshbooksTokens> tokenStore, ILogger<FreshbooksApiImpl> logger, IRestClientFactory factory)
        {
            _authorizer = authorizer;
            _authSettings = authSettings.CurrentValue;
            _appSettings = appSettings.CurrentValue;
            _tokenStore = tokenStore;
            _logger = logger;
            _factory = factory;
        }

        public async Task Authenticate()
        {
            var settings = _authSettings;

            var authCode =
                await _authorizer.StartAuthorization(settings.AuthorizationUrl, settings.RedirectUrl, settings.ClientId);

            _logger.LogInformation($"Received auth code {authCode}");
            
            var appS = _appSettings;
            var authS = _authSettings;
            var tokens = await _authorizer.RequestTokens(appS.ApiBaseUrl, authCode, authS.ClientId, authS.ClientSecret,
                authS.RedirectUrl);

            _logger.LogInformation($"Received bearer token and refresh token");
            await _tokenStore.StoreToken(tokens);
            _logger.LogInformation($"Authentication procedure complete!");

        }

        public async Task<IdentityInfo> GetIdentityInfo()
        {
            var url = $"{_appSettings.ApiBaseUrl}/auth/api/v1/users/me";
            var request = new RestRequest(url, Method.GET);
            request.AddHeader("Api-Version", "alpha")
                .AddHeader("Content-Type", MediaTypeNames.Application.Json);
            var client = await _factory.CreateRestClient();

            var response= await client.ExecuteGetAsync<IdentityInfo>(request: request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            
            throw new FreshbooksApiException(
                $"code={response.StatusCode.ToString()} message={response.Content}, error={response.ErrorMessage}");        }


        public async Task<TimeEntryResponse> GetTimeEntries(TimeEntryFilter filter, string businessId)
        {
            var url = $"{_appSettings.ApiBaseUrl}/timetracking/business/{businessId}/time_entries";
            var client = await _factory.CreateRestClient();
            var request = new RestRequest(url, Method.GET);

            request.AddParameter("page", 0)
                .AddParameter("started_from", filter.StartDateUtc.ToString("yyyy-MM-ddTHH:mm:ssZ"))
                .AddParameter("started_to", filter.EndDateUtc.ToString("yyyy-MM-ddTHH:mm:ssZ"));

            var response = await client.ExecuteGetAsync<TimeEntryResponse>(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            throw new FreshbooksApiException(
                $"code={response.StatusCode.ToString()} message={response.Content}, error={response.ErrorMessage}");        }
        
    }
}