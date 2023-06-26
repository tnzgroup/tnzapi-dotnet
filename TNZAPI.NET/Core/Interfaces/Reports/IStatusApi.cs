using TNZAPI.NET.Api.Reports.Status.Dto;

namespace TNZAPI.NET.Core.Interfaces.Reports
{
    public interface IStatusApi : IApiClientBase
    {
        StatusApiResult Poll(string messageID);
        StatusApiResult Poll(StatusRequestOptions options);
        StatusApiResult Poll(string messageID, IListRequestOptions listOptions = null);

        Task<StatusApiResult> PollAsync(string messageID);
        Task<StatusApiResult> PollAsync(StatusRequestOptions options);
        Task<StatusApiResult> PollAsync(string messageID, IListRequestOptions listOptions = null);
    }
}