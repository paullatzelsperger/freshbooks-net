using IdentityModel.OidcClient.Browser;

namespace RedirectServer.Browser
{
    internal class BrowserResult
    {
        public BrowserResultType ResultType { get; set; }
        public string Error { get; set; }
        public string Response { get; set; }
    }
}