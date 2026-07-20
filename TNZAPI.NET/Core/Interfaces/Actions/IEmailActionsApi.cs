using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IEmailActionsApi
    {
        EmailActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<EmailActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        EmailActionApiResult Abort(MessageID messageID);
        Task<EmailActionApiResult> AbortAsync(MessageID messageID);
        EmailActionApiResult Resubmit(MessageID messageID, DateTime sendTime);
        Task<EmailActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime);
    }
}