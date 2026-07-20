using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions.TTS
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class TTSActionsApi : ITTSActionsApi
    {
        private readonly TTSApi _tts;

        public TTSActionsApi(ITNZAuth auth)
        {
            _tts = new TTSApi(auth);
        }

        public TTSActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return _tts.Reschedule(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<TTSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            return _tts.RescheduleAsync(messageID, sendTime);
        }

        public TTSActionApiResult Abort(MessageID messageID)
        {
            return _tts.Abort(messageID);
        }

        [ComVisible(false)]
        public Task<TTSActionApiResult> AbortAsync(MessageID messageID)
        {
            return _tts.AbortAsync(messageID);
        }

        public TTSActionApiResult Resubmit(MessageID messageID, DateTime sendTime)
        {
            return _tts.Resubmit(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<TTSActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime)
        {
            return _tts.ResubmitAsync(messageID, sendTime);
        }

        public TTSActionApiResult Pacing(MessageID messageID, int numberOfOperators)
        {
            return _tts.Pacing(messageID, numberOfOperators);
        }

        [ComVisible(false)]
        public Task<TTSActionApiResult> PacingAsync(MessageID messageID, int numberOfOperators)
        {
            return _tts.PacingAsync(messageID, numberOfOperators);
        }
    }
}