namespace TNZAPI.NET.Demo.Api.Services
{
    public class SessionTokenProvider : ITokenProvider
    {
        private const string SessionKey = "TNZAuthTokenOverride";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public SessionTokenProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public string Resolve()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var overrideToken = session?.GetString(SessionKey);

            if (!string.IsNullOrEmpty(overrideToken))
            {
                return overrideToken;
            }

            return _configuration["TNZ_AUTH_TOKEN"] ?? "";
        }

        public void SetOverride(string token)
        {
            var session = _httpContextAccessor.HttpContext?.Session
                ?? throw new InvalidOperationException("Session is not available on the current request.");

            session.SetString(SessionKey, token);
        }
    }
}