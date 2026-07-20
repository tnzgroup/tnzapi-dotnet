using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Messaging.WhatsApp.Dto
{
    public class WhatsAppReceivedApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();

        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }

        public List<WhatsAppReceivedMessage> Messages { get; set; } = new List<WhatsAppReceivedMessage>();
    }

    public class WhatsAppReceivedMessage
    {
        public ReceivedID? ReceivedID { get; set; }
        public MessageID? MessageID { get; set; }
        public string? JobNum { get; set; }
        public string? SubAccount { get; set; }
        public string? Department { get; set; }
        public DateTime? ReceivedTimeLocal { get; set; }
        public DateTime? ReceivedTimeUTC { get; set; }
        public DateTime? ReceivedTimeUTC_RFC3339 { get; set; }
        public string? From { get; set; }
        public ContactID? ContactID { get; set; }
        public string? MessageText { get; set; }
        public string? Timezone { get; set; }
        public string? Version { get; set; }
    }
}
