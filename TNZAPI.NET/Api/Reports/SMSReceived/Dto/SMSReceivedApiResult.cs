using System.Xml.Serialization;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Helpers;

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

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }
    }

    [XmlType(TypeName = "Message")]
    public class SMSMessageReceived
    {
				[Obsolete("The ID is no longer supported. Please switch to using 'ReceivedID' instead.")]
				public int ID { get; set; }
        public ReceivedID ReceivedID { get; set; }
				public MessageID MessageID { get; set; }
				public ContactID ContactID { get; set; }
				public string JobNum { get; set; }
        public string SubAccount { get; set; }
        public string Department { get; set; }
        public DateTime Date 
        {
            get 
            {
                return DateUTC.ChangeToLocalDateTime();
            }
        }
        [XmlElement("ReceivedTimeUTC_RFC3339")]
        public DateTime DateUTC { get; set; }
        public string From { get; set; }
        public string MessageText { get; set; }
        public string Timezone { get; set; }
    }
}
