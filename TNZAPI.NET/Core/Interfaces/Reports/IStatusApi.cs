using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Reports.Status.Dto;

namespace TNZAPI.NET.Core.Interfaces.Reports
{
    public interface IStatusApi : IApiClientBase
    {
        StatusApiResult Poll(MessageID messageID);
        StatusApiResult Poll(StatusRequestOptions options);
        StatusApiResult Poll(MessageID messageID, IListRequestOptions listOptions = null);

        Task<StatusApiResult> PollAsync(MessageID messageID);
        Task<StatusApiResult> PollAsync(StatusRequestOptions options);
        Task<StatusApiResult> PollAsync(MessageID messageID, IListRequestOptions listOptions = null);

		#region Deprecated
		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		StatusApiResult Poll(string messageID);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		StatusApiResult Poll(string messageID, IListRequestOptions listOptions = null);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<StatusApiResult> PollAsync(string messageID);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<StatusApiResult> PollAsync(string messageID, IListRequestOptions listOptions = null);
		#endregion
	}
}