using TNZAPI.NET.Core.Interfaces.Actions;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IActionsApi
    {
        public IAbortApi Abort { get; set; }
        public IPacingApi Pacing { get; set; }
        public IRescheduleApi Reschedule { get; set; }
        public IResubmitApi Resubmit { get; set; }
    }
}
