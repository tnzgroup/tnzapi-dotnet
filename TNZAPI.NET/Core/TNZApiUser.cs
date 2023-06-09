namespace TNZAPI.NET.Core
{
    public class TNZApiUser : ITNZAuth
    {
        public string AuthToken { get; set; } = "";

        public string APIKey { get; set; } = "";
        public string Sender { get; set; } = "";
    }
}
