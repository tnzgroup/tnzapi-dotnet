using TNZAPI.NET.Core.Interfaces.Reports;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IReportsApi
    {
        IStatusApi Status { get; set; }
        ISMSReceivedApi SMSReceived { get; set; }
        ISMSReplyApi SMSReply { get; set; }
    }
}
