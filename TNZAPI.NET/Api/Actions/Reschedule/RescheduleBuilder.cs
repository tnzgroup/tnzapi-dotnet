using TNZAPI.NET.Api.Actions.Reschedule.Dto;

namespace TNZAPI.NET.Api.Actions.Reschedule
{
    public class RescheduleBuilder : IDisposable
    {
        private RescheduleRequestOptions Entity { get; set; }

        public RescheduleBuilder()
        {
            Entity = new RescheduleRequestOptions();
        }

        public RescheduleBuilder(string messageID)
        {
            Entity = new RescheduleRequestOptions()
            {
                MessageID = messageID
            };
        }

        /// <summary>
        /// Set message id
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>RescheduleBuilder</returns>
        public RescheduleBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = messageID;

            return this;
        }

        /// <summary>
        /// Set send time
        /// </summary>
        /// <param name="sendTime">DateTime value</param>
        /// <returns>RescheduleBuilder</returns>
        public RescheduleBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        /// <summary>
        /// Sets SendTime by using DateTime.TryParse() function
        /// </summary>
        /// <param name="sendTimeString">Send time</param>
        /// <returns>RescheduleBuilder</returns>
        public RescheduleBuilder SetSendTime(string sendTimeString)
        {
            DateTime sendTime;

            DateTime.TryParse(sendTimeString, out sendTime);

            Entity.SendTime = sendTime;

            return this;
        }

        /// <summary>
        /// Sets timezone
        /// </summary>
        /// <param name="timezone">Timezone</param>
        /// <returns>RescheduleBuilder</returns>
        public RescheduleBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        /// <summary>
        /// Build RescheduleRequestOptions
        /// </summary>
        /// <returns>RescheduleRequestOptions</returns>
        public RescheduleRequestOptions Build()
        {
            return Entity;
        }

        /// <summary>
        /// Build RescheduleRequestOptions (async)
        /// </summary>
        /// <returns>Task<RescheduleRequestOptions></returns>
        public async Task<RescheduleRequestOptions> BuildAsync()
        {
            return await Task.Run(() =>
            {
                return Entity;
            });
        }

        public void Dispose()
        {
            Entity = null;
        }
    }
}
