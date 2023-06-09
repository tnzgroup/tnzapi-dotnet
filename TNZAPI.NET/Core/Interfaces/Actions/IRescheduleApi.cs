using TNZAPI.NET.Api.Actions.Reschedule.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IRescheduleApi : IApiClientBase
    {
        RescheduleApiResult Submit(string messageID, DateTime sendTime);
        RescheduleApiResult Submit(RescheduleRequestOptions options);

        Task<RescheduleApiResult> SubmitAsync(string messageID, DateTime sendTime);
        Task<RescheduleApiResult> SubmitAsync(RescheduleRequestOptions options);
    }
}