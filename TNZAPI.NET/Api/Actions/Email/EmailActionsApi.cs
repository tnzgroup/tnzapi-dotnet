using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions.Email
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class EmailActionsApi : IEmailActionsApi
    {
        private readonly EmailApi _email;

        public EmailActionsApi(ITNZAuth auth)
        {
            _email = new EmailApi(auth);
        }

        public EmailActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return _email.Reschedule(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<EmailActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            return _email.RescheduleAsync(messageID, sendTime);
        }

        public EmailActionApiResult Abort(MessageID messageID)
        {
            return _email.Abort(messageID);
        }

        [ComVisible(false)]
        public Task<EmailActionApiResult> AbortAsync(MessageID messageID)
        {
            return _email.AbortAsync(messageID);
        }

        public EmailActionApiResult Resubmit(MessageID messageID, DateTime sendTime)
        {
            return _email.Resubmit(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<EmailActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime)
        {
            return _email.ResubmitAsync(messageID, sendTime);
        }
    }
}