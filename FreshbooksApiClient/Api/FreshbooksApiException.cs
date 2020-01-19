using System;

namespace FreshbooksApiClient.Api
{
    public class FreshbooksApiException : Exception
    {
        public FreshbooksApiException(string message) : base(message)
        {
        }
    }
}