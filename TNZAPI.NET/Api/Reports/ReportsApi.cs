using TNZAPI.NET.Api.Reports.SMSReceived;
using TNZAPI.NET.Api.Reports.SMSReply;
using TNZAPI.NET.Api.Reports.Status;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Reports;

namespace TNZAPI.NET.Api.Reports
{
    public class ReportsApi : IReportsApi
    {
        public IStatusApi Status { get; set; }
        public ISMSReceivedApi SMSReceived { get; set; }
        public ISMSReplyApi SMSReply { get; set; }

        public ReportsApi(ITNZAuth user)
        {
            Status = new StatusApi(user);
            SMSReceived = new SMSReceivedApi(user);
            SMSReply = new SMSReplyApi(user);
        }
    }
}
