using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record SmsAttachmentRequest(string FileName, string FileContent);

    // Every field SMSModel supports, aside from ContactID/GroupID (addressbook references, need a
    // contact/group picker rather than a text field — out of scope for this phase).
    public record SendSmsRequest(
        string ToNumber,
        string Message,
        string? Reference,
        string? TemplateId,
        string? NotificationType,
        string? WebhookCallbackUrl,
        string? WebhookCallbackFormat,
        string? ReportTo,
        string? SendTime,
        string? Timezone,
        string? SubAccount,
        string? Department,
        string? ChargeCode,
        string? FromNumber,
        string? SmsEmailReply,
        bool? CharacterConversion,
        ICollection<string>? FallbackMode,
        string? SendMode,
        ICollection<SmsAttachmentRequest>? Attachments
    );

    [ApiController]
    [Route("api/sms")]
    public class SmsController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public SmsController(TNZApiClient client)
        {
            _client = client;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendSmsRequest request)
        {
            try
            {
                var entity = new SMSModel
                {
                    Message = request.Message,
                    Destinations = new List<Destination> { new Destination { Recipient = request.ToNumber } },
                    Reference = request.Reference ?? "",
                    TemplateID = request.TemplateId ?? "",
                    WebhookCallbackURL = request.WebhookCallbackUrl ?? "",
                    ReportTo = request.ReportTo ?? "",
                    Timezone = request.Timezone ?? "",
                    SubAccount = request.SubAccount ?? "",
                    Department = request.Department ?? "",
                    ChargeCode = request.ChargeCode ?? "",
                    FromNumber = request.FromNumber ?? "",
                    SMSEmailReply = request.SmsEmailReply ?? "",
                    CharacterConversion = request.CharacterConversion ?? false,
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
                        .Where(m => Enum.TryParse<Enums.SMSFallbackMode>(m, out _))
                        .Select(m => Enum.Parse<Enums.SMSFallbackMode>(m))
                        .ToList();
                }

                if (request.Attachments is { Count: > 0 })
                {
                    entity.Files = request.Attachments
                        .Select(a => new Attachment(a.FileName, a.FileContent))
                        .ToList();
                }

                var result = await _client.Messaging.SMS.SendMessageAsync(entity);

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
                var result = await _client.Messaging.SMS.StatusAsync(new MessageID(id));

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        // SMSReply is SMS-only — no other channel's Status method takes recordsPerPage/page at all
        // (confirmed via SMSApi.cs: this is the same URL as Status, just with those two query params
        // added). Result shape is SMSStatusApiResult, same as Status — reply text lives in each
        // recipient's Recipients[].SMSReplies[].MessageText.
        [HttpGet("reply/{id}")]
        public async Task<IActionResult> Reply(string id, [FromQuery] int recordsPerPage = 100, [FromQuery] int page = 1)
        {
            try
            {
                if (this.ValidatePagination(page, recordsPerPage) is { } validationError)
                {
                    return validationError;
                }

                var result = await _client.Messaging.SMS.SMSReplyAsync(new MessageID(id), recordsPerPage, page);

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        // WhatsApp and RCS also have a Received endpoint (see WhatsAppController/RcsController) —
        // unlike SMSReply, which really is SMS-only. dateFrom (with optional dateTo, defaulting
        // server-side to now) takes precedence over timePeriod when supplied — SMSApi exposes this
        // as two distinct overloads, so we pick whichever one applies here based on whether dateFrom
        // parsed to a value.
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
                    ? await _client.Messaging.SMS.ReceivedAsync(parsedDateFrom.Value, parsedDateTo, recordsPerPage, page)
                    : await _client.Messaging.SMS.ReceivedAsync(timePeriod, recordsPerPage, page);

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}