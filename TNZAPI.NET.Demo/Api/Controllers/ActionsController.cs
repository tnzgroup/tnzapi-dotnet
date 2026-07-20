using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Demo.Api.Helpers;

namespace TNZAPI.NET.Demo.Api.Controllers
{
    public record AbortRequest(string MessageID, string Channel);
    public record RescheduleRequest(string MessageID, string Channel, string SendTime);
    public record ResubmitRequest(string MessageID, string Channel, string SendTime);
    public record PacingRequest(string MessageID, string Channel, int NumberOfOperators);

    // client.Actions is its own top-level facade (client.Actions.<Channel>), separate from
    // client.Messaging — not every channel supports every action: Abort/Reschedule exist on all 7
    // (SMS/Email/Fax/TTS/Voice/WhatsApp/RCS), Resubmit adds Email/Fax/TTS/Voice, Pacing is TTS/Voice
    // only, and Workflow has no Actions facade at all (confirmed via Core/Interfaces/Actions/*.cs —
    // TNZ's own tnzapi-ts-samples reference for this page is missing WhatsApp/RCS from its channel
    // list, which is a gap in that reference, not a real SDK limitation).
    [ApiController]
    [Route("api/actions")]
    public class ActionsController : ControllerBase
    {
        private readonly TNZApiClient _client;

        public ActionsController(TNZApiClient client)
        {
            _client = client;
        }

        [HttpPost("abort")]
        public async Task<IActionResult> Abort([FromBody] AbortRequest request)
        {
            try
            {
                var id = new MessageID(request.MessageID);

                IApiResult? result = request.Channel?.ToLowerInvariant() switch
                {
                    "sms" => await _client.Actions.SMS.AbortAsync(id),
                    "email" => await _client.Actions.Email.AbortAsync(id),
                    "fax" => await _client.Actions.Fax.AbortAsync(id),
                    "tts" => await _client.Actions.TTS.AbortAsync(id),
                    "voice" => await _client.Actions.Voice.AbortAsync(id),
                    "whatsapp" => await _client.Actions.WhatsApp.AbortAsync(id),
                    "rcs" => await _client.Actions.RCS.AbortAsync(id),
                    _ => null
                };

                if (result is null)
                {
                    return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Unknown channel: {request.Channel}" } });
                }

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPost("reschedule")]
        public async Task<IActionResult> Reschedule([FromBody] RescheduleRequest request)
        {
            try
            {
                // Explicit invariant culture — plain DateTime.TryParse resolves ambiguous formats
                // (e.g. "01/02/2025") using the server's current thread culture, which is
                // non-deterministic and can silently parse the wrong date. Matches the SDK's own
                // FlexibleDateTimeJsonConverter parsing convention.
                if (!DateTime.TryParse(request.SendTime, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var sendTime))
                {
                    return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Invalid SendTime: '{request.SendTime}' could not be parsed as a date/time." } });
                }

                var id = new MessageID(request.MessageID);

                IApiResult? result = request.Channel?.ToLowerInvariant() switch
                {
                    "sms" => await _client.Actions.SMS.RescheduleAsync(id, sendTime),
                    "email" => await _client.Actions.Email.RescheduleAsync(id, sendTime),
                    "fax" => await _client.Actions.Fax.RescheduleAsync(id, sendTime),
                    "tts" => await _client.Actions.TTS.RescheduleAsync(id, sendTime),
                    "voice" => await _client.Actions.Voice.RescheduleAsync(id, sendTime),
                    "whatsapp" => await _client.Actions.WhatsApp.RescheduleAsync(id, sendTime),
                    "rcs" => await _client.Actions.RCS.RescheduleAsync(id, sendTime),
                    _ => null
                };

                if (result is null)
                {
                    return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Unknown channel: {request.Channel}" } });
                }

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPost("resubmit")]
        public async Task<IActionResult> Resubmit([FromBody] ResubmitRequest request)
        {
            try
            {
                // Explicit invariant culture — plain DateTime.TryParse resolves ambiguous formats
                // (e.g. "01/02/2025") using the server's current thread culture, which is
                // non-deterministic and can silently parse the wrong date. Matches the SDK's own
                // FlexibleDateTimeJsonConverter parsing convention.
                if (!DateTime.TryParse(request.SendTime, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var sendTime))
                {
                    return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Invalid SendTime: '{request.SendTime}' could not be parsed as a date/time." } });
                }

                var id = new MessageID(request.MessageID);

                IApiResult? result = request.Channel?.ToLowerInvariant() switch
                {
                    "email" => await _client.Actions.Email.ResubmitAsync(id, sendTime),
                    "fax" => await _client.Actions.Fax.ResubmitAsync(id, sendTime),
                    "tts" => await _client.Actions.TTS.ResubmitAsync(id, sendTime),
                    "voice" => await _client.Actions.Voice.ResubmitAsync(id, sendTime),
                    _ => null
                };

                if (result is null)
                {
                    return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Unknown channel: {request.Channel}. Resubmit supports email, fax, tts, voice — not sms, whatsapp, or rcs." } });
                }

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }

        [HttpPost("pacing")]
        public async Task<IActionResult> Pacing([FromBody] PacingRequest request)
        {
            try
            {
                if (request.NumberOfOperators < 1)
                {
                    return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Invalid NumberOfOperators: '{request.NumberOfOperators}' must be at least 1." } });
                }

                var id = new MessageID(request.MessageID);

                IApiResult? result = request.Channel?.ToLowerInvariant() switch
                {
                    "tts" => await _client.Actions.TTS.PacingAsync(id, request.NumberOfOperators),
                    "voice" => await _client.Actions.Voice.PacingAsync(id, request.NumberOfOperators),
                    _ => null
                };

                if (result is null)
                {
                    return BadRequest(new { Result = "Failed", ErrorMessage = new[] { $"Unknown channel: {request.Channel}. Pacing supports tts, voice only." } });
                }

                return this.RespondWithResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "Failed", ErrorMessage = new[] { ex.Message } });
            }
        }
    }
}