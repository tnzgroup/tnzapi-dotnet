using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Voice;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions.Voice
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class VoiceActionsApi : IVoiceActionsApi
    {
        private readonly VoiceApi _voice;

        public VoiceActionsApi(ITNZAuth auth)
        {
            _voice = new VoiceApi(auth);
        }

        public VoiceActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return _voice.Reschedule(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<VoiceActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            return _voice.RescheduleAsync(messageID, sendTime);
        }

        public VoiceActionApiResult Abort(MessageID messageID)
        {
            return _voice.Abort(messageID);
        }

        [ComVisible(false)]
        public Task<VoiceActionApiResult> AbortAsync(MessageID messageID)
        {
            return _voice.AbortAsync(messageID);
        }

        public VoiceActionApiResult Resubmit(MessageID messageID, DateTime sendTime)
        {
            return _voice.Resubmit(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<VoiceActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime)
        {
            return _voice.ResubmitAsync(messageID, sendTime);
        }

        public VoiceActionApiResult Pacing(MessageID messageID, int numberOfOperators)
        {
            return _voice.Pacing(messageID, numberOfOperators);
        }

        [ComVisible(false)]
        public Task<VoiceActionApiResult> PacingAsync(MessageID messageID, int numberOfOperators)
        {
            return _voice.PacingAsync(messageID, numberOfOperators);
        }
    }
}