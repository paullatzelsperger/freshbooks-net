namespace FreshbooksTimeEntryGenerator.Contracts
{
    public class FreshbooksAuthSettings
    {
        public string AuthorizationUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}