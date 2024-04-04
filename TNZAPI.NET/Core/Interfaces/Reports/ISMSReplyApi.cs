using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Reports.SMSReply.Dto;

namespace TNZAPI.NET.Core.Interfaces.Reports
{
    public interface ISMSReplyApi : IApiClientBase
    {
        SMSReplyApiResult Poll(MessageID messageID);
        SMSReplyApiResult Poll(SMSReplyRequestOptions options);
        SMSReplyApiResult Poll(MessageID messageID, IListRequestOptions listOptions = null);

        Task<SMSReplyApiResult> PollAsync(MessageID messageID);
        Task<SMSReplyApiResult> PollAsync(SMSReplyRequestOptions options);
        Task<SMSReplyApiResult> PollAsync(MessageID messageID, IListRequestOptions listOptions = null);

		#region Deprecated
		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		SMSReplyApiResult Poll(string messageID);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		SMSReplyApiResult Poll(string messageID, IListRequestOptions listOptions = null);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<SMSReplyApiResult> PollAsync(string messageID);

		[Obsolete("The messageID of type 'string' is no longer supported. Please switch to using type 'MessageID' instead.")]
		Task<SMSReplyApiResult> PollAsync(string messageID, IListRequestOptions listOptions = null);
		#endregion
	}
}