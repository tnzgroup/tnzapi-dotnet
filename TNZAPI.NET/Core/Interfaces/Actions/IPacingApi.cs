using TNZAPI.NET.Api.Actions.Pacing.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IPacingApi : IApiClientBase
    {
        PacingApiResult Submit(MessageID messageID, int numberOfOperators);
        PacingApiResult Submit(PacingRequestOptions options);

        Task<PacingApiResult> SubmitAsync(MessageID messageID, int numberOfOperators);
        Task<PacingApiResult> SubmitAsync(PacingRequestOptions options);

		#region Deprecated
		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		PacingApiResult Submit(string messageID, int numberOfOperators);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<PacingApiResult> SubmitAsync(string messageID, int numberOfOperators);
		#endregion
	}
}