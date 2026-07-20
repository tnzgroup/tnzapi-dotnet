using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IActionsApi
    {
        ISMSActionsApi SMS { get; set; }
        IEmailActionsApi Email { get; set; }
        ITTSActionsApi TTS { get; set; }
        IVoiceActionsApi Voice { get; set; }
        IFaxActionsApi Fax { get; set; }
        IWhatsAppActionsApi WhatsApp { get; set; }
        IRCSActionsApi RCS { get; set; }
    }
}