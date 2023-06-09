namespace TNZAPI.NET.Core
{
    public interface ITNZAuth
    {
        string AuthToken { get; set; }

        string APIKey { get; set; }
        string Sender { get; set; }
    }
}
