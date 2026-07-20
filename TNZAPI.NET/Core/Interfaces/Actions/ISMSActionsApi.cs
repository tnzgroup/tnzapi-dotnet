using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface ISMSActionsApi
    {
        SMSActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<SMSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        SMSActionApiResult Abort(MessageID messageID);
        Task<SMSActionApiResult> AbortAsync(MessageID messageID);
    }
}