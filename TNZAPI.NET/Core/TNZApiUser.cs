namespace TNZAPI.NET.Core
{
    public class TNZApiUser : ITNZAuth
    {
        public string AuthToken { get; set; } = Environment.GetEnvironmentVariable("TNZ_AUTH_TOKEN") ?? "";
    }
}