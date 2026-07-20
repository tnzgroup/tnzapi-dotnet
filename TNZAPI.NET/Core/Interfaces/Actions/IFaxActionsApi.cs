using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IFaxActionsApi
    {
        FaxActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<FaxActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        FaxActionApiResult Abort(MessageID messageID);
        Task<FaxActionApiResult> AbortAsync(MessageID messageID);
        FaxActionApiResult Resubmit(MessageID messageID, DateTime sendTime);
        Task<FaxActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime);
    }
}