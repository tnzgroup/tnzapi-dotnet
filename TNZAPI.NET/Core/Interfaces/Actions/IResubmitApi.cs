using TNZAPI.NET.Api.Actions.Resubmit.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IResubmitApi : IApiClientBase
    {
        ResubmitApiResult Submit(string messageID);
        ResubmitApiResult Submit(string messageID, DateTime sendTime);
        ResubmitApiResult Submit(ResubmitRequestOptions options);

        Task<ResubmitApiResult> SubmitAsync(string messageID);
        Task<ResubmitApiResult> SubmitAsync(string messageID, DateTime sendTime);
        Task<ResubmitApiResult> SubmitAsync(ResubmitRequestOptions options);
    }
}