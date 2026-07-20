using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Messaging.Voice.Dto
{
    public class VoiceStatusApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public List<string> ErrorMessage { get; set; } = new List<string>();

        public MessageID? MessageID { get; set; }
        public Enums.JobStatus JobStatus { get; set; }
        public string? JobNum { get; set; }
        public string? Account { get; set; }
        public string? SubAccount { get; set; }
        public string? Department { get; set; }
        public string? Reference { get; set; }
        public DateTime? CreatedTimeLocal { get; set; }
        public DateTime? CreatedTimeUTC { get; set; }
        public DateTime? DelayedTimeLocal { get; set; }
        public DateTime? DelayedTimeUTC { get; set; }
        public string? Timezone { get; set; }
        public int Count { get; set; }
        public int Complete { get; set; }
        public int Success { get; set; }
        public int Failed { get; set; }
        public decimal? Price { get; set; }

        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }

        public List<VoiceRecipientResult> Recipients { get; set; } = new List<VoiceRecipientResult>();
    }

    public class VoiceRecipientResult
    {
        public Enums.RecipientChannelType Type { get; set; }
        public string? DestSeq { get; set; }
        public string? Destination { get; set; }
        public ContactID? ContactID { get; set; }
        public Enums.MessageStatus Status { get; set; }
        public string? Result { get; set; }
        public DateTime? SentTimeLocal { get; set; }
        public DateTime? SentTimeUTC { get; set; }
        public string? Attention { get; set; }
        public string? Company { get; set; }
        public string? Custom1 { get; set; }
        public string? Custom2 { get; set; }
        public string? Custom3 { get; set; }
        public string? Custom4 { get; set; }
        public string? Custom5 { get; set; }
        public string? Custom6 { get; set; }
        public string? Custom7 { get; set; }
        public string? Custom8 { get; set; }
        public string? Custom9 { get; set; }
        public string? RemoteID { get; set; }
        public decimal? Price { get; set; }
    }
}