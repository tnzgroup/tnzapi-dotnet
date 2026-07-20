using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface ITTSActionsApi
    {
        TTSActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<TTSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        TTSActionApiResult Abort(MessageID messageID);
        Task<TTSActionApiResult> AbortAsync(MessageID messageID);
        TTSActionApiResult Resubmit(MessageID messageID, DateTime sendTime);
        Task<TTSActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime);
        TTSActionApiResult Pacing(MessageID messageID, int numberOfOperators);
        Task<TTSActionApiResult> PacingAsync(MessageID messageID, int numberOfOperators);
    }
}