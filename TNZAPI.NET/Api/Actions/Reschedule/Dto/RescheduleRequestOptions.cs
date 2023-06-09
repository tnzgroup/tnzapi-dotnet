namespace TNZAPI.NET.Api.Actions.Reschedule.Dto
{
    public class RescheduleRequestOptions
    {
        public string MessageID { get; set; } = "";

        public DateTime SendTime { get; set; } = DateTime.Now;
    }
}
