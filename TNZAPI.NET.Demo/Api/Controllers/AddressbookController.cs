using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    // Every field ContactModel supports except ViewBy/EditBy/AccessControl (enum pickers, out of
    // scope for this phase — left server-default) and Timezone (the tnzapi-ts-samples reference's
    // Contacts page includes a Timezone create field, but .NET's ContactModel has no such settable
    // property — it's a read-only server-computed field on ContactDetail, not something the SDK lets
    // a caller set on Create/Update. That's a real gap in the reference, not faked here.)
    public record ContactRequest(
        string? FirstName,
        string? LastName,
        string? Company,
        string? MobilePhone,
        string? EmailAddress,
        string? FaxNumber,
        string? Title,
        string? Position,
        string? Attention,
        string? RecipDepartment,
        string? StreetAddress,
        string? Suburb,
        string? City,
        string? State,
        string? Country,
        string? Postcode,
        string? MainPhone,
        string? WebAddress,
        string? Custom1,
        string? Custom2,
        string? Custom3,
        string? Custom4,
        string? Notes
    );

    [ApiController]
    [Route("api/addressbook/contacts")]
    public class AddressbookController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public AddressbookController(TNZApiClient client)
        {
            _client = client;
        }

        // existing is the current server-side values (from Details), used on Update so that a field
        // omitted from the request (null) keeps its current value instead of being cleared to "" —
        // ContactModel's string properties are non-nullable with no wire concept of "leave
        // untouched", so the only way to give this HTTP PATCH endpoint real partial-update semantics
        // is to fill omitted fields from the existing record before sending the full object back.
        private static ContactModel ToModel(ContactRequest request, ContactDetail? existing = null)
        {
            return new ContactModel
            {
                FirstName = request.FirstName ?? existing?.FirstName ?? "",
                LastName = request.LastName ?? existing?.LastName ?? "",
                Company = request.Company ?? existing?.Company ?? "",
                MobilePhone = request.MobilePhone ?? existing?.MobilePhone ?? "",
                EmailAddress = request.EmailAddress ?? existing?.EmailAddress ?? "",
                FaxNumber = request.FaxNumber ?? existing?.FaxNumber ?? "",
                Title = request.Title ?? existing?.Title ?? "",
                Position = request.Position ?? existing?.Position ?? "",
                Attention = request.Attention ?? existing?.Attention ?? "",
                RecipDepartment = request.RecipDepartment ?? existing?.RecipDepartment ?? "",
                StreetAddress = request.StreetAddress ?? existing?.StreetAddress ?? "",
                Suburb = request.Suburb ?? existing?.Suburb ?? "",
                City = request.City ?? existing?.City ?? "",
                State = request.State ?? existing?.State ?? "",
                Country = request.Country ?? existing?.Country ?? "",
                Postcode = request.Postcode ?? existing?.Postcode ?? "",
                MainPhone = request.MainPhone ?? existing?.MainPhone ?? "",
                WebAddress = request.WebAddress ?? existing?.WebAddress ?? "",
                Custom1 = request.Custom1 ?? existing?.Custom1 ?? "",
                Custom2 = request.Custom2 ?? existing?.Custom2 ?? "",
                Custom3 = request.Custom3 ?? existing?.Custom3 ?? "",
                Custom4 = request.Custom4 ?? existing?.Custom4 ?? "",
                Notes = request.Notes ?? existing?.Notes ?? "",
            };
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

                var result = await _client.Addressbook.Contact.ListAsync(page, recordsPerPage);
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactRequest request)
        {
            try
            {
                var result = await _client.Addressbook.Contact.CreateAsync(ToModel(request));
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
                var result = await _client.Addressbook.Contact.DetailsAsync(new ContactID(id));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] ContactRequest request)
        {
            try
            {
                var contactID = new ContactID(id);

                var existing = await _client.Addressbook.Contact.DetailsAsync(contactID);
                if (existing.Result != Enums.ResultCode.Success)
                {
                    return this.RespondWithResult(existing);
                }

                var result = await _client.Addressbook.Contact.UpdateAsync(contactID, ToModel(request, existing));
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
                var result = await _client.Addressbook.Contact.DeleteAsync(new ContactID(id));
                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}