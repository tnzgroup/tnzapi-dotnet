using TNZAPI.NET.Api.Actions.Email;
using TNZAPI.NET.Api.Actions.Fax;
using TNZAPI.NET.Api.Actions.RCS;
using TNZAPI.NET.Api.Actions.SMS;
using TNZAPI.NET.Api.Actions.TTS;
using TNZAPI.NET.Api.Actions.Voice;
using TNZAPI.NET.Api.Actions.WhatsApp;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions
{
    public class ActionsApi : IActionsApi
    {
        public ISMSActionsApi SMS { get; set; }
        public IEmailActionsApi Email { get; set; }
        public ITTSActionsApi TTS { get; set; }
        public IVoiceActionsApi Voice { get; set; }
        public IFaxActionsApi Fax { get; set; }
        public IWhatsAppActionsApi WhatsApp { get; set; }
        public IRCSActionsApi RCS { get; set; }

        public ActionsApi(ITNZAuth auth)
        {
            SMS = new SMSActionsApi(auth);
            Email = new EmailActionsApi(auth);
            TTS = new TTSActionsApi(auth);
            Voice = new VoiceActionsApi(auth);
            Fax = new FaxActionsApi(auth);
            WhatsApp = new WhatsAppActionsApi(auth);
            RCS = new RCSActionsApi(auth);
        }
    }
}