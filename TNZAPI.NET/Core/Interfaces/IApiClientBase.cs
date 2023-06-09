using TNZAPI.NET.Core;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IApiClientBase
    {
        void SetAPIUser(ITNZAuth apiUser);
        void SetAuthToken(string authToken);
        void SetAPISender(string apiSender);
        void SetAPIKey(string apiKey);
    }
}
