using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.WhatsApp.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IWhatsAppActionsApi
    {
        WhatsAppActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<WhatsAppActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        WhatsAppActionApiResult Abort(MessageID messageID);
        Task<WhatsAppActionApiResult> AbortAsync(MessageID messageID);
    }
}