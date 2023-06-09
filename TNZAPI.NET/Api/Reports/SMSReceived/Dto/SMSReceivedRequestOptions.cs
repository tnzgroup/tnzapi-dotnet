namespace TNZAPI.NET.Api.Reports.SMSReceived.Dto
{
    public class SMSReceivedRequestOptions
    {
        public int TimePeriod { get; set; } = 1440; // in min
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int RecordsPerPage { get; set; } = 100;
        public int Page { get; set; } = 1;
    }
}
