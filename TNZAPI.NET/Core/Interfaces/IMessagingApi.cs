using TNZAPI.NET.Core.Interfaces.Messaging;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IMessagingApi
    {
        ISMSApi SMS { get; set; }
        IEmailApi Email { get; set; }
        ITTSApi TTS { get; set; }
        IVoiceApi Voice { get; set; }
        IFaxApi Fax { get; set; }
        IWhatsAppApi WhatsApp { get; set; }
        IRCSApi RCS { get; set; }
        IWorkflowApi Workflow { get; set; }
    }
}