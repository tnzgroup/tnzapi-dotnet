using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.RCS;
using TNZAPI.NET.Api.Messaging.RCS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions.RCS
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class RCSActionsApi : IRCSActionsApi
    {
        private readonly RCSApi _rcs;

        public RCSActionsApi(ITNZAuth auth)
        {
            _rcs = new RCSApi(auth);
        }

        public RCSActionApiResult Reschedule(MessageID messageID, DateTime sendTime)
        {
            return _rcs.Reschedule(messageID, sendTime);
        }

        [ComVisible(false)]
        public Task<RCSActionApiResult> RescheduleAsync(MessageID messageID, DateTime sendTime)
        {
            return _rcs.RescheduleAsync(messageID, sendTime);
        }

        public RCSActionApiResult Abort(MessageID messageID)
        {
            return _rcs.Abort(messageID);
        }

        [ComVisible(false)]
        public Task<RCSActionApiResult> AbortAsync(MessageID messageID)
        {
            return _rcs.AbortAsync(messageID);
        }
    }
}