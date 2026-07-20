namespace TNZAPI.NET.Core
{
    internal class TNZApiConfig
    {
        internal static string Domain
        {
            get
            {
                var value = Environment.GetEnvironmentVariable("TNZ_API_URL");

                return string.IsNullOrEmpty(value) ? "https://api.tnz.co.nz" : value.TrimEnd('/');
            }
        }
        internal static string Version = "3.00";
        internal static string UserAgent = $"TNZAPI.NET-{Version}";
    }
}