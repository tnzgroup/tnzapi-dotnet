﻿using System.Xml.Serialization;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Reports.Status.Dto
{
    [XmlType(TypeName = "root")]
    public class StatusApiResult : IApiResult
    {
        public Enums.ResultCode Result { get; set; }
        public MessageID MessageID { get; set; }
        public Enums.StatusCode Status { get; set; } = Enums.StatusCode.Unknown;
        public string JobNum { get; set; } = "";
        public string Account { get; set; } = "";
        public string SubAccount { get; set; } = "";
        public string Department { get; set; } = "";
        public string Reference { get; set; } = "";
        public DateTime Created {
            get
            {
                return CreatedUTC.ChangeToLocalDateTime();
            }
        }
        [XmlElement("CreatedTimeUTC_RFC3339")]
        public DateTime CreatedUTC { get; set; } = new DateTime();
        public DateTime Delayed
        {
            get
            {
                return DelayedUTC.ChangeToLocalDateTime();
            }
        }
        [XmlElement("DelayedTimeUTC_RFC3339")]
        public DateTime DelayedUTC { get; set; } = new DateTime();
        public string Timezone { get; set; } = "New Zealand";
        public int Count { get; set; } = 0;
        public int Complete { get; set; } = 0;
        public int Success { get; set; } = 0;
        public int Failed { get; set; } = 0;
        public double? Price { get; set; } = 0;

        public int TotalRecords { get; set; } = 0;
        public int RecordsPerPage { get; set; } = 0;
        public int PageCount { get; set; } = 0;
        public int Page { get; set; } = 0;

        [XmlArray("Recipients")]
        public List<MessageRecipient> Recipients { get; set; }

        [XmlElement("ErrorMessage")]
        public List<string> ErrorMessage { get; set; }

        public string GetStatusString()
        {
            return Enum.GetName(typeof(Enums.StatusCode), Status);
        }

        public string GetStatusString(Enums.StatusCode code)
        {
            return Enum.GetName(typeof(Enums.StatusCode), code);
        }
    }

    [XmlType(TypeName = "Recipient")]
    public class MessageRecipient
    {
        public Enums.MessageType Type { get; set; }
        public string DestSeq { get; set; }
        public string Destination { get; set; }
        public Enums.ResultCode Status { get; set; }
        public string Result { get; set; }

        [Obsolete("The SentDate is no longer supported. Please switch to using 'SentTime' instead.")]
        public DateTime SentDate => SentTime;

        public DateTime SentTime
        {
            get
            {
                return SentTimeUTC.ChangeToLocalDateTime();
            }
        }

        [XmlElement("SentTimeUTC_RFC3339")]
        public DateTime SentTimeUTC { get; set; } = new DateTime();
        public string Attention { get; set; }
        public string Company { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }
        public string Custom6 { get; set; }
        public string Custom7 { get; set; }
        public string Custom8 { get; set; }
        public string Custom9 { get; set; }
        public string RemoteID { get; set; }
        public double? Price { get; set; }

    }

    public class SMSReply
    {
        public DateTime Date { get; set; }
        public DateTime DateUTC { get; set; }
        public string From { get; set; }
        public string MessageText { get; set; }
    }

}
