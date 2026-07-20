using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    // Unlike TTS, PlayFile/File are meaningful here — Voice's KeypadModel actually uses them for
    // file-based keypad playback.
    public record VoiceKeypadRequest(int Tone, string? Play, string? RouteNumber, string? PlaySection, string? PlayFile, string? File);

    // Every field VoiceModel supports, aside from ContactID/GroupID (addressbook references, need a
    // contact/group picker rather than a text field — out of scope for this phase) and VoiceFiles
    // (undocumented in docs/voice.md — its relationship to Keypad.PlayFile isn't established
    // anywhere in the SDK, so it was dropped from the UI as confusing rather than guessed at).
    // Unlike TTS, there's no text-to-speech step — MessageToPeople/MessageToAnswerPhones/
    // CallRouteMessage*/EndCallMessage are all base64 audio content from the frontend's
    // SingleFileField, not literal spoken text, even though they're plain strings same as TTS's
    // identically-named fields. The SDK's own client-side validation (VoiceApi.cs) requires
    // MessageToPeople or TemplateID to be non-empty.
    public record SendVoiceRequest(
        string ToNumber,
        string? MessageToPeople,
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
        string? MessageToAnswerPhones,
        string? AnswerPhoneMode,
        ICollection<VoiceKeypadRequest>? Keypads,
        bool? KeypadOptionRequired,
        string? CallRouteMessageOnWrongKey,
        string? CallRouteMessageToPeople,
        string? CallRouteMessageToOperators,
        string? EndCallMessage,
        int? NumberOfOperators,
        int? RetryAttempts,
        int? RetryPeriod,
        string? CallerId,
        string? Options,
        string? SendMode
    );

    [ApiController]
    [Route("api/voice")]
    public class VoiceController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public VoiceController(TNZApiClient client)
        {
            _client = client;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] SendVoiceRequest request)
        {
            try
            {
                var entity = new VoiceModel
                {
                    MessageToPeople = request.MessageToPeople ?? "",
                    Destinations = new List<Destination> { new Destination { Recipient = request.ToNumber } },
                    Reference = request.Reference ?? "",
                    TemplateID = request.TemplateId ?? "",
                    WebhookCallbackURL = request.WebhookCallbackUrl ?? "",
                    ReportTo = request.ReportTo ?? "",
                    Timezone = request.Timezone ?? "",
                    SubAccount = request.SubAccount ?? "",
                    Department = request.Department ?? "",
                    ChargeCode = request.ChargeCode ?? "",
                    MessageToAnswerPhones = request.MessageToAnswerPhones ?? "",
                    KeypadOptionRequired = request.KeypadOptionRequired ?? false,
                    CallRouteMessageOnWrongKey = request.CallRouteMessageOnWrongKey ?? "",
                    CallRouteMessageToPeople = request.CallRouteMessageToPeople ?? "",
                    CallRouteMessageToOperators = request.CallRouteMessageToOperators ?? "",
                    EndCallMessage = request.EndCallMessage ?? "",
                    NumberOfOperators = request.NumberOfOperators,
                    RetryAttempts = request.RetryAttempts,
                    RetryPeriod = request.RetryPeriod,
                    CallerID = request.CallerId ?? "",
                    Options = request.Options ?? "",
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
                if (Enum.TryParse<Enums.AnswerPhoneMode>(request.AnswerPhoneMode, out var answerPhoneMode))
                {
                    entity.AnswerPhoneMode = answerPhoneMode;
                }

                if (request.Keypads is { Count: > 0 })
                {
                    var keypads = new List<KeypadModel>();
                    foreach (var k in request.Keypads)
                    {
                        var keypad = new KeypadModel
                        {
                            Tone = k.Tone,
                            Play = k.Play ?? "",
                            RouteNumber = k.RouteNumber ?? "",
                            PlayFile = k.PlayFile ?? "",
                            File = k.File ?? "",
                        };
                        if (Enum.TryParse<Enums.KeypadPlaySection>(k.PlaySection, out var section))
                        {
                            keypad.PlaySection = section;
                        }
                        keypads.Add(keypad);
                    }
                    entity.Keypads = keypads;
                }

                var result = await _client.Messaging.Voice.SendMessageAsync(entity);

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
                var result = await _client.Messaging.Voice.StatusAsync(new MessageID(id));

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}