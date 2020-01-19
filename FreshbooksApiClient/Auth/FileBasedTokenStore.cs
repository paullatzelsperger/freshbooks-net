using System.IO;
using System.Text;
using System.Threading.Tasks;
using FreshbooksApiClient.Api;
using FreshbooksApiClient.Contracts;
using Newtonsoft.Json;

namespace FreshbooksApiClient.Auth
{
    public class FileBasedTokenStore<T> : ITokenStore<T> where T : new()
    {
        private const string Filename = "token.json";

        public async Task StoreToken(T response)
        {
            var json = JsonConvert.SerializeObject(response);
            await Task.Run(() => { File.WriteAllText(Filename, json, Encoding.UTF8); });
        }


        public async Task<T> GetTokenAsync()
        {
            if (!File.Exists(Filename))
            {
                return await Task.FromResult(new T());
            }

            try
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<T>(File.ReadAllText(Filename)));
            }
            catch (JsonException)
            {
                File.Delete(Filename);
                return await Task.FromResult(new T());
            }

        }
    }
}