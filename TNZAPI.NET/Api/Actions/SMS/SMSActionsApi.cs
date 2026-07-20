using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions.SMS
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class SMSActionsApi : ISMSActionsApi
    {
        private readonly SMSApi _sms;

        public SMSActionsApi(ITNZAuth auth)
        {
            _sms = new SMSApi(auth);
        }

        public SMSActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return _sms.Reschedule(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<SMSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            return _sms.RescheduleAsync(messageID, sendTime);
        }

        public SMSActionApiResult Abort(MessageID messageID)
        {
            return _sms.Abort(messageID);
        }

        [ComVisible(false)]
        public Task<SMSActionApiResult> AbortAsync(MessageID messageID)
        {
            return _sms.AbortAsync(messageID);
        }
    }
}