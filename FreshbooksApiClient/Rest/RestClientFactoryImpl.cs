using System.Threading.Tasks;
using FreshbooksApiClient.Api;
using FreshbooksApiClient.Contracts;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization;

namespace FreshbooksApiClient.Rest
{
    public class RestClientFactoryImpl : IRestClientFactory
    {
        private readonly ITokenStore<FreshbooksTokens> _tokenDatabase;

        public RestClientFactoryImpl(ITokenStore<FreshbooksTokens> tokenDatabase)
        {
            _tokenDatabase = tokenDatabase;
        }

        public async Task<IRestClient> CreateRestClient()
        {
            // create a new RestClient using the Canto Access Token as Bearer Token and a custom JsonSerializer.
            // the custom JsonSerializer is needed in order to be able to influence how enums are serialized.

            var tokens = await _tokenDatabase.GetTokenAsync();
            return new RestClient
            {
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(tokens.AccessToken, "Bearer"),
                
            }.UseSerializer(() => new JsonNetSerializer());
        }

        private class JsonNetSerializer : IRestSerializer
        {
            public string Serialize(object obj) => 
                JsonConvert.SerializeObject(obj);

            public string Serialize(Parameter parameter) => 
                JsonConvert.SerializeObject(parameter.Value);

            public T Deserialize<T>(IRestResponse response) => 
                JsonConvert.DeserializeObject<T>(response.Content);

            public string[] SupportedContentTypes { get; } =
            {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
            };

            public string ContentType { get; set; } = "application/json";

            public DataFormat DataFormat { get; } = DataFormat.Json;
        }
    }
}