using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record FaxAttachmentRequest(string FileName, string FileContent);

    // Every field FaxModel supports, aside from ContactID/GroupID (addressbook references, need a
    // contact/group picker rather than a text field — out of scope for this phase). Fax has no
    // message-text field of its own — the document to send is always an attachment.
    public record SendFaxRequest(
        string ToNumber,
        ICollection<FaxAttachmentRequest>? Attachments,
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
        string? Csid,
        string? Resolution,
        string? WatermarkFolder,
        string? WatermarkFirstPage,
        string? WatermarkAllPages,
        int? RetryAttempts,
        int? RetryPeriod,
        string? SendMode
    );

    [ApiController]
    [Route("api/fax")]
    public class FaxController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public FaxController(TNZApiClient client)
        {
            _client = client;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendFaxRequest request)
        {
            try
            {
                var entity = new FaxModel
                {
                    Destinations = new List<Destination> { new Destination { Recipient = request.ToNumber } },
                    Reference = request.Reference ?? "",
                    TemplateID = request.TemplateId ?? "",
                    WebhookCallbackURL = request.WebhookCallbackUrl ?? "",
                    ReportTo = request.ReportTo ?? "",
                    Timezone = request.Timezone ?? "",
                    SubAccount = request.SubAccount ?? "",
                    Department = request.Department ?? "",
                    ChargeCode = request.ChargeCode ?? "",
                    CSID = request.Csid ?? "",
                    WatermarkFolder = request.WatermarkFolder ?? "",
                    WatermarkFirstPage = request.WatermarkFirstPage ?? "",
                    WatermarkAllPages = request.WatermarkAllPages ?? "",
                    RetryAttempts = request.RetryAttempts,
                    RetryPeriod = request.RetryPeriod,
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
                if (Enum.TryParse<Enums.FaxResolution>(request.Resolution, out var resolution))
                {
                    entity.Resolution = resolution;
                }

                if (request.Attachments is { Count: > 0 })
                {
                    entity.Files = request.Attachments
                        .Select(a => new Attachment(a.FileName, a.FileContent))
                        .ToList();
                }

                var result = await _client.Messaging.Fax.SendMessageAsync(entity);

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
                var result = await _client.Messaging.Fax.StatusAsync(new MessageID(id));

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}