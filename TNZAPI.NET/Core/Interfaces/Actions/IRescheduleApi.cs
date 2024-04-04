using TNZAPI.NET.Api.Actions.Reschedule.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Core.Interfaces.Actions
{
    public interface IRescheduleApi : IApiClientBase
    {
        RescheduleApiResult Submit(MessageID messageID, DateTime sendTime, string timezone = null);
        RescheduleApiResult Submit(RescheduleRequestOptions options);

        Task<RescheduleApiResult> SubmitAsync(MessageID messageID, DateTime sendTime, string timezone = null);
        Task<RescheduleApiResult> SubmitAsync(RescheduleRequestOptions options);

		#region Deprecated
		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		RescheduleApiResult Submit(string messageID, DateTime sendTime, string timezone = null);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<RescheduleApiResult> SubmitAsync(string messageID, DateTime sendTime, string timezone = null);
		#endregion
	}
}