using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record GroupContactRequest(string GroupID, string ContactID);

    // client.Addressbook.Group.Contact is the inverse of Contact.Group (same many-to-many relation,
    // looked up from the Group side). Same two real gaps vs. tnzapi-ts-samples' reference page as
    // ContactGroupsController: .NET's Add(GroupID, ContactID) has no GroupCode parameter, and
    // List(GroupID) takes no page/recordsPerPage.
    [ApiController]
    [Route("api/addressbook/group-contacts")]
    public class GroupContactsController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public GroupContactsController(TNZApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string groupID)
        {
            try
            {
                var result = await _client.Addressbook.Group.Contact.ListAsync(new GroupID(groupID));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GroupContactRequest request)
        {
            try
            {
                var result = await _client.Addressbook.Group.Contact.AddAsync(new GroupID(request.GroupID), new ContactID(request.ContactID));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] GroupContactRequest request)
        {
            try
            {
                var result = await _client.Addressbook.Group.Contact.RemoveAsync(new GroupID(request.GroupID), new ContactID(request.ContactID));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}