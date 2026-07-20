using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Core;
using SdkHttpRequest = TNZAPI.NET.Helpers.HttpRequest;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record SetApiUrlRequest(string ApiUrl);
    public record SetSslVerificationRequest(bool Enabled);
    public record SetAllowInsecureHttpRequest(bool Enabled);

    [ApiController]
    [Route("api/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly IHostEnvironment _environment;

        public SettingsController(IHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet("api-url")]
        public IActionResult GetApiUrl()
        {
            return Ok(new { ApiUrl = TNZApiConfig.Domain });
        }

        [HttpPost("api-url")]
        public IActionResult SetApiUrl([FromBody] SetApiUrlRequest request)
        {
            try
            {
                // Every mutating endpoint on this controller changes process-wide state (see the comments
                // below) — an unauthenticated caller who can reach these outside local development could
                // repoint the whole process's outbound TNZ API traffic at an attacker-controlled host, after
                // which the next request from ANY session sends its Bearer JWT there. Fail closed outside
                // Development rather than relying on network-level access control alone.
                if (!_environment.IsDevelopment())
                {
                    return StatusCode(403, new { Error = "This setting can only be changed when ASPNETCORE_ENVIRONMENT is Development." });
                }

                if (string.IsNullOrWhiteSpace(request.ApiUrl))
                {
                    return BadRequest(new { Error = "API URL must not be empty." });
                }

                if (!Uri.TryCreate(request.ApiUrl, UriKind.Absolute, out var uri)
                    || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    return BadRequest(new { Error = "API URL must be a valid absolute http:// or https:// URL." });
                }

                // TNZApiConfig.Domain reads this OS environment variable directly — this is process-wide,
                // affecting every concurrent session on this backend instance, not just the caller's own
                // session (unlike the AuthToken override, which TNZApiClient takes as a constructor
                // parameter and so can genuinely be scoped per-request). Acceptable for a single-developer
                // local demo; would need a different mechanism for a multi-tenant deployment.
                Environment.SetEnvironmentVariable("TNZ_API_URL", request.ApiUrl);

                return Ok(new { Status = "ok", ApiUrl = TNZApiConfig.Domain });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("ssl-verification")]
        public IActionResult GetSslVerification()
        {
            // Read the actual handler rather than tracking a separate bool alongside it — a
            // HttpClientHandler's ServerCertificateCustomValidationCallback is directly inspectable,
            // so there's no need for a second, independently-maintained source of truth that could
            // drift from what MessageHandler is actually configured with.
            var enabled = SdkHttpRequest.MessageHandler is HttpClientHandler handler
                && handler.ServerCertificateCustomValidationCallback is null;

            return Ok(new { Enabled = enabled });
        }

        [HttpPost("ssl-verification")]
        public IActionResult SetSslVerification([FromBody] SetSslVerificationRequest request)
        {
            try
            {
                if (!_environment.IsDevelopment())
                {
                    return StatusCode(403, new { Error = "This setting can only be changed when ASPNETCORE_ENVIRONMENT is Development." });
                }

                // HttpRequest.MessageHandler is a test seam the SDK exposes only for swapping in a fake
                // handler in unit tests — there is no documented production API for this. It's the only
                // hook available to disable certificate validation, and like TNZ_API_URL it's a static
                // field, so this is process-wide, not scoped to the caller's session — every concurrent
                // request on this backend instance reads the same MessageHandler, with no locking, so one
                // session's toggle can race with another session's in-flight SDK call. Acceptable for a
                // single-developer local demo; would need a different mechanism for a multi-tenant
                // deployment. Irrelevant when the API URL is plain http:// — certificate validation only
                // ever happens during a TLS handshake, so this toggle has no effect unless the URL is https://.
                SdkHttpRequest.MessageHandler = request.Enabled
                    ? new HttpClientHandler()
                    : new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                    };

                return Ok(new { Status = "ok", Enabled = request.Enabled });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("allow-insecure-http")]
        public IActionResult GetAllowInsecureHttp()
        {
            var enabled = string.Equals(
                Environment.GetEnvironmentVariable("TNZ_ALLOW_INSECURE_HTTP"), "true", StringComparison.OrdinalIgnoreCase);

            return Ok(new { Enabled = enabled });
        }

        [HttpPost("allow-insecure-http")]
        public IActionResult SetAllowInsecureHttp([FromBody] SetAllowInsecureHttpRequest request)
        {
            try
            {
                if (!_environment.IsDevelopment())
                {
                    return StatusCode(403, new { Error = "This setting can only be changed when ASPNETCORE_ENVIRONMENT is Development." });
                }

                // A deliberately separate toggle from SSL Verification above, not inferred from the API URL's
                // scheme — HttpRequest.cs already refuses any non-https:// URL client-side unless this exact
                // env var is set to "true", and that refusal is the whole point: it forces an explicit,
                // second opt-in before the bearer token can ever be sent over plaintext, rather than silently
                // escalating just because someone typed (or mistyped) a http:// URL. Process-wide, like the
                // two settings above.
                Environment.SetEnvironmentVariable("TNZ_ALLOW_INSECURE_HTTP", request.Enabled ? "true" : "false");

                return Ok(new { Status = "ok", Enabled = request.Enabled });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}