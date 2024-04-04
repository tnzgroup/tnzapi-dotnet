using TNZAPI.NET.Api.Actions.Abort.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IAbortApi : IApiClientBase
    {
        AbortApiResult Submit(MessageID messageID);
        AbortApiResult Submit(AbortRequestOptions options);

        Task<AbortApiResult> SubmitAsync(MessageID messageID);
        Task<AbortApiResult> SubmitAsync(AbortRequestOptions options);

		#region Deprecated
		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		AbortApiResult Submit(string messageID);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<AbortApiResult> SubmitAsync(string messageID);
		#endregion
	}
}