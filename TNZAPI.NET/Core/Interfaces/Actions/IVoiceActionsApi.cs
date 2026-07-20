using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Voice.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IVoiceActionsApi
    {
        VoiceActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<VoiceActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        VoiceActionApiResult Abort(MessageID messageID);
        Task<VoiceActionApiResult> AbortAsync(MessageID messageID);
        VoiceActionApiResult Resubmit(MessageID messageID, DateTime sendTime);
        Task<VoiceActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime);
        VoiceActionApiResult Pacing(MessageID messageID, int numberOfOperators);
        Task<VoiceActionApiResult> PacingAsync(MessageID messageID, int numberOfOperators);
    }
}