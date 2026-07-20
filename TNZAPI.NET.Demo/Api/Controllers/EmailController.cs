using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record EmailAttachmentRequest(string FileName, string FileContent);

    // Every field EmailModel supports, aside from ContactID/GroupID (addressbook references, need
    // a contact/group picker rather than a text field — out of scope for this phase).
    public record SendEmailRequest(
        string EmailAddress,
        string Subject,
        string MessageHtml,
        string? Reference,
        string? TemplateId,
        string? SmtpFrom,
        string? From,
        string? FromEmail,
        string? ReplyTo,
        string? CcEmail,
        string? WebhookCallbackUrl,
        string? WebhookCallbackFormat,
        string? ReportTo,
        string? SendTime,
        string? Timezone,
        string? SubAccount,
        string? Department,
        string? ChargeCode,
        string? NotificationType,
        string? SendMode,
        ICollection<EmailAttachmentRequest>? Attachments
    );

    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public EmailController(TNZApiClient client)
        {
            _client = client;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendEmailRequest request)
        {
            try
            {
                var entity = new EmailModel
                {
                    EmailSubject = request.Subject,
                    MessageHTML = request.MessageHtml,
                    Destinations = new List<Destination> { new Destination { Recipient = request.EmailAddress } },
                    Reference = request.Reference ?? "",
                    TemplateID = request.TemplateId ?? "",
                    SMTPFrom = request.SmtpFrom ?? "",
                    From = request.From ?? "",
                    FromEmail = request.FromEmail ?? "",
                    ReplyTo = request.ReplyTo ?? "",
                    CCEmail = request.CcEmail ?? "",
                    WebhookCallbackURL = request.WebhookCallbackUrl ?? "",
                    ReportTo = request.ReportTo ?? "",
                    Timezone = request.Timezone ?? "",
                    SubAccount = request.SubAccount ?? "",
                    Department = request.Department ?? "",
                    ChargeCode = request.ChargeCode ?? "",
                };

                if (request.Attachments is { Count: > 0 })
                {
                    entity.Files = request.Attachments
                        .Select(a => new Attachment(a.FileName, a.FileContent))
                        .ToList();
                }

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

                var result = await _client.Messaging.Email.SendMessageAsync(entity);

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
                var result = await _client.Messaging.Email.StatusAsync(new MessageID(id));

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}