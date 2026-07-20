using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.RCS.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IRCSActionsApi
    {
        RCSActionApiResult Reschedule(MessageID messageID, DateTime sendTime);
        Task<RCSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime);
        RCSActionApiResult Abort(MessageID messageID);
        Task<RCSActionApiResult> AbortAsync(MessageID messageID);
    }
}