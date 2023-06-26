using TNZAPI.NET.Api.Reports.SMSReply.Dto;

namespace TNZAPI.NET.Core.Interfaces.Reports
{
    public interface ISMSReplyApi : IApiClientBase
    {
        SMSReplyApiResult Poll(string messageID);
        SMSReplyApiResult Poll(SMSReplyRequestOptions options);
        SMSReplyApiResult Poll(string messageID, IListRequestOptions listOptions = null);

        Task<SMSReplyApiResult> PollAsync(string messageID);
        Task<SMSReplyApiResult> PollAsync(SMSReplyRequestOptions options);
        Task<SMSReplyApiResult> PollAsync(string messageID, IListRequestOptions listOptions = null);
    }
}