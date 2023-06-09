using TNZAPI.NET.Api.Actions.Abort.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IAbortApi : IApiClientBase
    {
        AbortApiResult Submit(string messageID);
        AbortApiResult Submit(AbortRequestOptions options);

        Task<AbortApiResult> SubmitAsync(string messageID);
        Task<AbortApiResult> SubmitAsync(AbortRequestOptions options);
    }
}