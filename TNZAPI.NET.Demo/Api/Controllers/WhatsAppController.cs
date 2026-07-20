using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.WhatsApp.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record WhatsAppAttachmentRequest(string FileName, string FileContent);

    // Every field WhatsAppModel supports, aside from ContactID/GroupID (addressbook references,
    // need a contact/group picker rather than a text field — out of scope for this phase).
    // Custom1-9 are per-recipient personalisation values substituted into the approved template's
    // [[Custom1]]..[[Custom9]] tokens (docs/whatsapp.md) — set on Recipient, not on the model itself.
    public record SendWhatsAppRequest(
        string ToNumber,
        string Message,
        ICollection<WhatsAppAttachmentRequest>? Attachments,
        string? Reference,
        string? TemplateId,
        ICollection<string>? FallbackMode,
        string? FromNumber,
        string? Custom1,
        string? Custom2,
        string? Custom3,
        string? Custom4,
        string? Custom5,
        string? Custom6,
        string? Custom7,
        string? Custom8,
        string? Custom9,
        string? NotificationType,
        string? WebhookCallbackUrl,
        string? WebhookCallbackFormat,
        string? ReportTo,
        string? SendTime,
        string? Timezone,
        string? SubAccount,
        string? Department,
        string? ChargeCode,
        string? SendMode
    );

    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsAppController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public WhatsAppController(TNZApiClient client)
        {
            _client = client;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendWhatsAppRequest request)
        {
            try
            {
                var entity = new WhatsAppModel
                {
                    Message = request.Message,
                    Destinations = new List<Destination>
                    {
                        new Destination
                        {
                            Recipient = request.ToNumber,
                            Custom1 = request.Custom1 ?? "",
                            Custom2 = request.Custom2 ?? "",
                            Custom3 = request.Custom3 ?? "",
                            Custom4 = request.Custom4 ?? "",
                            Custom5 = request.Custom5 ?? "",
                            Custom6 = request.Custom6 ?? "",
                            Custom7 = request.Custom7 ?? "",
                            Custom8 = request.Custom8 ?? "",
                            Custom9 = request.Custom9 ?? "",
                        },
                    },
                    Reference = request.Reference ?? "",
                    TemplateID = request.TemplateId ?? "",
                    FromNumber = request.FromNumber ?? "",
                    WebhookCallbackURL = request.WebhookCallbackUrl ?? "",
                    ReportTo = request.ReportTo ?? "",
                    Timezone = request.Timezone ?? "",
                    SubAccount = request.SubAccount ?? "",
                    Department = request.Department ?? "",
                    ChargeCode = request.ChargeCode ?? "",
                };

                if (!string.IsNullOrWhiteSpace(request.SendTime))
                {
                    if (!DateTime.TryParse(request.SendTime, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var sendTime))
                    {
                        return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Invalid SendTime: '{request.SendTime}' could not be parsed as a date/time." } });
                    }
                    entity.SendTime = sendTime;
                }
                if (Enum.TryParse<Enums.WebhookCallbackType>(request.WebhookCallbackFormat, out var webhookFormat))
                {
                    entity.WebhookCallbackFormat = webhookFormat;
                }
                if (Enum.TryParse<Enums.NotificationType>(request.NotificationType, out var notificationType))
                {
                    entity.NotificationType = notificationType;
                }
                if (Enum.TryParse<Enums.SendModeType>(request.SendMode, out var sendMode))
                {
                    entity.SendMode = sendMode;
                }

                if (request.FallbackMode is { Count: > 0 })
                {
                    entity.FallbackMode = request.FallbackMode
                        .Where(m => Enum.TryParse<Enums.WhatsAppFallbackMode>(m, out _))
                        .Select(m => Enum.Parse<Enums.WhatsAppFallbackMode>(m))
                        .ToList();
                }

                if (request.Attachments is { Count: > 0 })
                {
                    entity.Files = request.Attachments
                        .Select(a => new Attachment(a.FileName, a.FileContent))
                        .ToList();
                }

                var result = await _client.Messaging.WhatsApp.SendMessageAsync(entity);

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpGet("status/{id}")]
        public async Task<IActionResult> Status(string id)
        {
            try
            {
                var result = await _client.Messaging.WhatsApp.StatusAsync(new MessageID(id));

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        // dateFrom (with optional dateTo, defaulting server-side to now) takes precedence over
        // timePeriod when supplied — WhatsAppApi exposes this as two distinct overloads, so we pick
        // whichever one applies here based on whether dateFrom parsed to a value. Same pattern as
        // SmsController.Received.
        [HttpGet("received")]
        public async Task<IActionResult> Received(
            [FromQuery] int timePeriod = 1440,
            [FromQuery] int recordsPerPage = 100,
            [FromQuery] int page = 1,
            [FromQuery] string? dateFrom = null,
            [FromQuery] string? dateTo = null)
        {
            try
            {
                if (this.ValidatePagination(page, recordsPerPage) is { } validationError)
                {
                    return validationError;
                }

                DateTime? parsedDateFrom = null;
                if (!string.IsNullOrWhiteSpace(dateFrom))
                {
                    if (!DateTime.TryParse(dateFrom, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var from))
                    {
                        return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Invalid dateFrom: '{dateFrom}' could not be parsed as a date/time." } });
                    }
                    parsedDateFrom = from;
                }

                DateTime? parsedDateTo = null;
                if (!string.IsNullOrWhiteSpace(dateTo))
                {
                    if (!DateTime.TryParse(dateTo, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var to))
                    {
                        return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Invalid dateTo: '{dateTo}' could not be parsed as a date/time." } });
                    }
                    parsedDateTo = to;
                }

                if (parsedDateFrom is null && parsedDateTo is not null)
                {
                    return BadRequest(new { Result = "Failed", ErrorMessage = new[] { "dateTo was supplied without dateFrom — both are required together." } });
                }

                var result = parsedDateFrom is not null
                    ? await _client.Messaging.WhatsApp.ReceivedAsync(parsedDateFrom.Value, parsedDateTo, recordsPerPage, page)
                    : await _client.Messaging.WhatsApp.ReceivedAsync(timePeriod, recordsPerPage, page);

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}
