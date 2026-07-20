using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record ContactGroupRequest(string ContactID, string GroupID);

    // client.Addressbook.Contact.Group is the many-to-many relation facade between Contact and
    // Group. Two real gaps vs. tnzapi-ts-samples' reference page, both left as gaps rather than
    // faked: (1) .NET's Add(ContactID, GroupID) has no GroupCode parameter — the ts SDK lets a
    // caller add by GroupCode as an alternative to GroupID, .NET's IContactGroupApi does not, so
    // there's no "Group Code" field here; (2) .NET's List(ContactID) takes no page/recordsPerPage —
    // the ts SDK's List accepts pagination, .NET's doesn't, so this always returns every group for
    // the contact in one call.
    [ApiController]
    [Route("api/addressbook/contact-groups")]
    public class ContactGroupsController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public ContactGroupsController(TNZApiClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string contactID)
        {
            try
            {
                var result = await _client.Addressbook.Contact.Group.ListAsync(new ContactID(contactID));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ContactGroupRequest request)
        {
            try
            {
                var result = await _client.Addressbook.Contact.Group.AddAsync(new ContactID(request.ContactID), new GroupID(request.GroupID));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] ContactGroupRequest request)
        {
            try
            {
                var result = await _client.Addressbook.Contact.Group.RemoveAsync(new ContactID(request.ContactID), new GroupID(request.GroupID));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}