namespace TNZAPI.NET.Demo.Api.Services
{
    public interface ITokenProvider
    {
        string Resolve();
        void SetOverride(string token);
    }
}