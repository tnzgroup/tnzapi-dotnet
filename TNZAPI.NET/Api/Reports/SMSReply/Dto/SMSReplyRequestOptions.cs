namespace TNZAPI.NET.Api.Reports.SMSReply.Dto
{
    public class SMSReplyRequestOptions
    {
        public string MessageID { get; set; } = "";
        public int RecordsPerPage { get; set; } = 100;
        public int Page { get; set; } = 1;
    }
}
