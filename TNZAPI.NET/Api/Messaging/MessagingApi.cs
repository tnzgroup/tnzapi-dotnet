using TNZAPI.NET.Api.Messaging.Email;
using TNZAPI.NET.Api.Messaging.Fax;
using TNZAPI.NET.Api.Messaging.RCS;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Api.Messaging.TTS;
using TNZAPI.NET.Api.Messaging.Voice;
using TNZAPI.NET.Api.Messaging.WhatsApp;
using TNZAPI.NET.Api.Messaging.Workflow;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Messaging;

namespace TNZAPI.NET.Api.Messaging
{
    public class MessagingApi : IMessagingApi
    {
        public ISMSApi SMS { get; set; }
        public IEmailApi Email { get; set; }
        public ITTSApi TTS { get; set; }
        public IVoiceApi Voice { get; set; }
        public IFaxApi Fax { get; set; }
        public IWhatsAppApi WhatsApp { get; set; }
        public IRCSApi RCS { get; set; }
        public IWorkflowApi Workflow { get; set; }

        public MessagingApi(ITNZAuth auth)
        {
            SMS = new SMSApi(auth);
            Email = new EmailApi(auth);
            TTS = new TTSApi(auth);
            Voice = new VoiceApi(auth);
            Fax = new FaxApi(auth);
            WhatsApp = new WhatsAppApi(auth);
            RCS = new RCSApi(auth);
            Workflow = new WorkflowApi(auth);
        }
    }
}