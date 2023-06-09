using System.Xml.Serialization;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Api.Reports.SMSReceived.Dto
{
    [XmlType(TypeName = "root")]
    public class SMSReceivedApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public int TotalRecords { get; set; }
        public int RecordsPerPage { get; set; }
        public int PageCount { get; set; }
        public int Page { get; set; }
        [XmlArray("Messages")]
        public List<SMSMessageReceived> Messages { get; set; }
        public List<string> ErrorMessage { get; set; }
    }

    [XmlType(TypeName = "Message")]
    public class SMSMessageReceived
    {
        public int ID { get; set; }
        public string MessageID { get; set; }
        public string JobNum { get; set; }
        public string SubAccount { get; set; }
        public string Department { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateUTC { get; set; }
        public string From { get; set; }
        public string MessageText { get; set; }
        public string Timezone { get; set; }
    }
}
