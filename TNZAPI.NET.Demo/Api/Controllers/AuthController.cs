using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Demo.Api.Services;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record SetTokenRequest(string Token);

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenProvider _tokenProvider;

        public AuthController(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        [HttpPost("token")]
        public IActionResult SetToken([FromBody] SetTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest(new { Error = "Token must not be empty." });
            }

            if (request.Token.Split('.').Length != 3)
            {
                return BadRequest(new { Error = "Token does not look like a JWT (expected three dot-separated segments)." });
            }

            try
            {
                _tokenProvider.SetOverride(request.Token);
            }
            catch (InvalidOperationException ex)
            {
                // SessionTokenProvider.SetOverride throws if Session is unavailable on this request.
                return StatusCode(500, new { Error = ex.Message });
            }

            return Ok(new { Status = "ok" });
        }
    }
}
