using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Workflow.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    // Workflow is genuinely different from every other channel: no Message/TemplateID text content
    // (it triggers a pre-configured, no-code Workflow Template built in the Dashboard), no Status/
    // Received/Actions — just Send (docs/workflow.md). Its recipients are also unusually flexible:
    // ToNumber/MainPhone/EmailAddress can all be set on the SAME recipient (omni-channel — the
    // Template decides which channel(s) to use), and ContactID/GroupID addressbook references are a
    // first-class recipient mechanism here, not an out-of-scope extra like other channels.
    public record SendWorkflowRequest(
        string WorkflowTemplateId,
        string? ToNumber,
        string? MainPhone,
        string? EmailAddress,
        string? ContactIds,
        string? GroupIds,
        string? Reference,
        string? NotificationType,
        string? WebhookCallbackUrl,
        string? WebhookCallbackFormat,
        string? SubAccount,
        string? Department,
        string? ChargeCode,
        string? SendTime,
        string? Timezone,
        string? SendMode
    );

    [ApiController]
    [Route("api/workflow")]
    public class WorkflowController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public WorkflowController(TNZApiClient client)
        {
            _client = client;
        }

        private static IEnumerable<string> SplitIds(string? value)
        {
            return (value ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendWorkflowRequest request)
        {
            try
            {
                var destinations = new List<Destination>();

                if (!string.IsNullOrWhiteSpace(request.ToNumber)
                    || !string.IsNullOrWhiteSpace(request.MainPhone)
                    || !string.IsNullOrWhiteSpace(request.EmailAddress))
                {
                    // Unlike other channels, Workflow's wire object reads ToNumber/MainPhone/
                    // EmailAddress directly off Destination (not the primary Recipient field) — see
                    // docs/workflow.md's "Destination fields (Destination)" section.
                    destinations.Add(new Destination
                    {
                        ToNumber = request.ToNumber ?? "",
                        MainPhone = request.MainPhone ?? "",
                        EmailAddress = request.EmailAddress ?? "",
                    });
                }

                foreach (var id in SplitIds(request.ContactIds))
                {
                    destinations.Add(new Destination(new ContactID(id)));
                }

                foreach (var id in SplitIds(request.GroupIds))
                {
                    destinations.Add(new Destination(new GroupID(id)));
                }

                var entity = new WorkflowModel
                {
                    WorkflowTemplateID = request.WorkflowTemplateId,
                    Destinations = destinations,
                    Reference = request.Reference ?? "",
                    WebhookCallbackURL = request.WebhookCallbackUrl ?? "",
                    SubAccount = request.SubAccount ?? "",
                    Department = request.Department ?? "",
                    ChargeCode = request.ChargeCode ?? "",
                    Timezone = request.Timezone ?? "",
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

                var result = await _client.Messaging.Workflow.SendMessageAsync(entity);

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}