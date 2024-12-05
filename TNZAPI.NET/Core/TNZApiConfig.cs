namespace TNZAPI.NET.Core
{
    internal class TNZApiConfig
    {
#if DEBUG
				internal static string Domain = "http://localhost:8060";
				// internal static string Domain = "https://localhost:7283";
#else
		internal static string Domain = "https://api.tnz.co.nz";
#endif
		
				//internal static string Domain = "https://api.tnz.co.nz";
				internal static string Version = "2.04";
        internal static string UserAgent = $"TNZAPI.NET-{Version}";
    }
}
