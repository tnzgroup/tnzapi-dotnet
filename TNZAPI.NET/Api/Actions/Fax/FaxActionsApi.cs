using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions.Fax
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class FaxActionsApi : IFaxActionsApi
    {
        private readonly FaxApi _fax;

        public FaxActionsApi(ITNZAuth auth)
        {
            _fax = new FaxApi(auth);
        }

        public FaxActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return _fax.Reschedule(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<FaxActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            return _fax.RescheduleAsync(messageID, sendTime);
        }

        public FaxActionApiResult Abort(MessageID messageID)
        {
            return _fax.Abort(messageID);
        }

        [ComVisible(false)]
        public Task<FaxActionApiResult> AbortAsync(MessageID messageID)
        {
            return _fax.AbortAsync(messageID);
        }

        public FaxActionApiResult Resubmit(MessageID messageID, DateTime sendTime)
        {
            return _fax.Resubmit(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<FaxActionApiResult> ResubmitAsync(MessageID messageID, DateTime sendTime)
        {
            return _fax.ResubmitAsync(messageID, sendTime);
        }
    }
}