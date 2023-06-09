using TNZAPI.NET.Api.Messaging.Email;
using TNZAPI.NET.Api.Messaging.Fax;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Api.Messaging.TTS;
using TNZAPI.NET.Api.Messaging.Voice;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Messaging;

namespace TNZAPI.NET.Api.Messaging
{
    public class MessagingApi : IMessagingApi
    {
        public IEmailApi Email { get; set; }
        public IFaxApi Fax { get; set; }
        public ISMSApi SMS { get; set; }
        public ITTSApi TTS { get; set; }
        public IVoiceApi Voice { get; set; }

        public MessagingApi(ITNZAuth user)
        {
            Email = new EmailApi(user);
            Fax = new FaxApi(user);
            SMS = new SMSApi(user);
            TTS = new TTSApi(user);
            Voice = new VoiceApi(user);
        }
    }
}
