using TNZAPI.NET.Core.Interfaces.Reports;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IReportsApi
    {
        ISMSReceivedApi SMSReceived { get; set; }
        IStatusApi Status { get; set; }
    }
}
