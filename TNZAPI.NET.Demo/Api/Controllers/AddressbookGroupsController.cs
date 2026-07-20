using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    // Every field GroupModel supports. ViewEditBy accepts the same enum names as the wire format
    // (Account/SubAccount/Department/No) — left as free text like every other enum-typed field
    // elsewhere in this Demo (SendMode, NotificationType, ...), parsed server-side and simply
    // ignored if it doesn't match one of those names.
    public record GroupRequest(
        string? GroupName,
        string? SubAccount,
        string? Department,
        string? ViewEditBy
    );

    [ApiController]
    [Route("api/addressbook/groups")]
    public class AddressbookGroupsController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public AddressbookGroupsController(TNZApiClient client)
        {
            _client = client;
        }

        // existing is the current server-side values (from Details), used on Update so that a field
        // omitted from the request (null) keeps its current value instead of being cleared to "" —
        // GroupModel's string properties are non-nullable with no wire concept of "leave untouched",
        // so the only way to give this HTTP PATCH endpoint real partial-update semantics is to fill
        // omitted fields from the existing record before sending the full object back.
        private static GroupModel ToModel(GroupRequest request, GroupDetail? existing = null)
        {
            var entity = new GroupModel
            {
                GroupName = request.GroupName ?? existing?.GroupName ?? "",
                SubAccount = request.SubAccount ?? existing?.SubAccount ?? "",
                Department = request.Department ?? existing?.Department ?? "",
            };

            if (Enum.TryParse<Enums.ViewEditByOptions>(request.ViewEditBy, out var viewEditBy))
            {
                entity.ViewEditBy = viewEditBy;
            }
            else
            {
                entity.ViewEditBy = existing?.ViewEditBy;
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

                var result = await _client.Addressbook.Group.ListAsync(page, recordsPerPage);
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupRequest request)
        {
            try
            {
                var result = await _client.Addressbook.Group.CreateAsync(ToModel(request));
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
                var result = await _client.Addressbook.Group.DetailsAsync(new GroupID(id));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] GroupRequest request)
        {
            try
            {
                var groupID = new GroupID(id);

                var existing = await _client.Addressbook.Group.DetailsAsync(groupID);
                if (existing.Result != Enums.ResultCode.Success)
                {
                    return this.RespondWithResult(existing);
                }

                var result = await _client.Addressbook.Group.UpdateAsync(groupID, ToModel(request, existing));
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
                var result = await _client.Addressbook.Group.DeleteAsync(new GroupID(id));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}