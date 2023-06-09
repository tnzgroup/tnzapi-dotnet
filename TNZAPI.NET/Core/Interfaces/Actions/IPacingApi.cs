using TNZAPI.NET.Api.Actions.Pacing.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IPacingApi : IApiClientBase
    {
        PacingApiResult Submit(string messageID, int numberOfOperators);
        PacingApiResult Submit(PacingRequestOptions options);

        Task<PacingApiResult> SubmitAsync(string messageID, int numberOfOperators);
        Task<PacingApiResult> SubmitAsync(PacingRequestOptions options);
    }
}