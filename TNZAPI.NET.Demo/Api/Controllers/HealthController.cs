using Microsoft.AspNetCore.Mvc;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { Status = "ok" });
    }
}
