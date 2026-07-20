using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Configuration.OptOut.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    // Every field OptOutModel supports — matches tnzapi-ts-samples' reference page's field set
    // exactly, no gaps. client.Configuration.OptOut is the facade (see CLAUDE.md's
    // client.Configuration -> OptOut mapping), not a top-level client.OptOut like the ts reference's
    // subtitle text implies — that's just descriptive wording, not a capability difference.
    public record OptOutRequest(
        string? Destination,
        string? DestType,
        string? Department,
        string? SubAccount,
        string? ContactID,
        string? StopMessage,
        string? Notes
    );

    [ApiController]
    [Route("api/optout")]
    public class OptOutController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public OptOutController(TNZApiClient client)
        {
            _client = client;
        }

        private static OptOutModel ToModel(OptOutRequest request)
        {
            var entity = new OptOutModel
            {
                Destination = request.Destination ?? "",
                DestType = request.DestType ?? "",
                Department = request.Department ?? "",
                SubAccount = request.SubAccount ?? "",
                StopMessage = request.StopMessage ?? "",
                Notes = request.Notes ?? "",
            };

            if (!string.IsNullOrWhiteSpace(request.ContactID))
            {
                entity.ContactID = new ContactID(request.ContactID);
            }

            return entity;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int recordsPerPage = 100)
        {
            try
            {
                if (this.ValidatePagination(page, recordsPerPage) is { } validationError)
                {
                    return validationError;
                }

                var result = await _client.Configuration.OptOut.ListAsync(page: page, recordsPerPage: recordsPerPage);
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OptOutRequest request)
        {
            try
            {
                var result = await _client.Configuration.OptOut.CreateAsync(ToModel(request));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var result = await _client.Configuration.OptOut.DetailsAsync(new OptOutID(id));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _client.Configuration.OptOut.DeleteAsync(new OptOutID(id));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}