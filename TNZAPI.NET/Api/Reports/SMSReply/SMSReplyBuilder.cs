﻿using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Reports.SMSReply.Dto;
using TNZAPI.NET.Core.Builders;

namespace TNZAPI.NET.Api.Reports.SMSReply
{
    public class SMSReplyBuilder : ReportRequestOptionBuilder<SMSReplyRequestOptions>
    {
        public SMSReplyBuilder()
        {
        }

        public SMSReplyBuilder(string messageID) : base(messageID)
        {
            
        }

        public SMSReplyBuilder(MessageID messageID) : base(messageID)
        {

        }
    }
}
