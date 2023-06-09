using TNZAPI.NET.Api.Reports.SMSReceived;
using TNZAPI.NET.Api.Reports.Status;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Reports;

namespace TNZAPI.NET.Api.Reports
{
    public class ReportsApi : IReportsApi
    {
        public ISMSReceivedApi SMSReceived { get; set; }
        public IStatusApi Status { get; set; }

        public ReportsApi(ITNZAuth user)
        {
            SMSReceived = new SMSReceivedApi(user);
            Status = new StatusApi(user);
        }
    }
}
