using TNZAPI.NET.Api.Reports.Status.Dto;

namespace TNZAPI.NET.Core.Interfaces.Reports
{
    public interface IStatusApi : IApiClientBase
    {
        StatusApiResult Poll(string messageID);
        StatusApiResult Poll(StatusRequestOptions options);

        Task<StatusApiResult> PollAsync(string messageID);
        Task<StatusApiResult> PollAsync(StatusRequestOptions options);
    }
}