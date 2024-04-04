using TNZAPI.NET.Api.Actions.Resubmit.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IResubmitApi : IApiClientBase
    {
        ResubmitApiResult Submit(MessageID messageID);
        ResubmitApiResult Submit(MessageID messageID, DateTime sendTime, string timezone = null);
        ResubmitApiResult Submit(ResubmitRequestOptions options);

        Task<ResubmitApiResult> SubmitAsync(MessageID messageID);
        Task<ResubmitApiResult> SubmitAsync(MessageID messageID, DateTime sendTime, string timezone = null);
        Task<ResubmitApiResult> SubmitAsync(ResubmitRequestOptions options);

		#region Deprecated
		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		ResubmitApiResult Submit(string messageID);
		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		ResubmitApiResult Submit(string messageID, DateTime sendTime, string timezone = null);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<ResubmitApiResult> SubmitAsync(string messageID);
		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<ResubmitApiResult> SubmitAsync(string messageID, DateTime sendTime, string timezone = null);
        #endregion
    }
}