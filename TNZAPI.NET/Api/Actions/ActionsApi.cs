using TNZAPI.NET.Api.Actions.Abort;
using TNZAPI.NET.Api.Actions.Pacing;
using TNZAPI.NET.Api.Actions.Reschedule;
using TNZAPI.NET.Api.Actions.Resubmit;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Api.Actions
{
    internal class ActionsApi : IActionsApi
    {
        public IAbortApi Abort { get; set; }
        public IPacingApi Pacing { get; set; }
        public IRescheduleApi Reschedule { get; set; }
        public IResubmitApi Resubmit { get; set; }

        public ActionsApi(ITNZAuth user)
        {
            Abort = new AbortApi(user);
            Pacing = new PacingApi(user);
            Reschedule = new RescheduleApi(user);
            Resubmit = new ResubmitApi(user);
        }
    }
}
