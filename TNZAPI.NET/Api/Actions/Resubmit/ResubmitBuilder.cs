using TNZAPI.NET.Api.Actions.Resubmit.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Actions.Reschedule
{
    public class ResubmitBuilder : IDisposable
    {
        private ResubmitRequestOptions Entity { get; set; }

        public ResubmitBuilder()
        {
            Entity = new ResubmitRequestOptions();
        }

        public ResubmitBuilder(string messageID)
        {
            Entity = new ResubmitRequestOptions()
            {
                MessageID = new MessageID(messageID)
            };
        }

		public ResubmitBuilder(MessageID messageID)
		{
			Entity = new ResubmitRequestOptions()
			{
				MessageID =messageID
			};
		}

		/// <summary>
		/// Set message id
		/// </summary>
		/// <param name="messageID">MessageID</param>
		/// <returns>ResubmitBuilder</returns>
		public ResubmitBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

		/// <summary>
		/// Set message id
		/// </summary>
		/// <param name="messageID">MessageID</param>
		/// <returns>ResubmitBuilder</returns>
		public ResubmitBuilder SetMessageID(MessageID messageID)
		{
			Entity.MessageID = messageID;

			return this;
		}

		/// <summary>
		/// Set send time
		/// </summary>
		/// <param name="sendTime">DateTime value</param>
		/// <returns>ResubmitBuilder</returns>
		public ResubmitBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        /// <summary>
        /// Sets SendTime by using DateTime.TryParse() function
        /// </summary>
        /// <param name="sendTimeString">Send time</param>
        /// <returns>ResubmitBuilder</returns>
        public ResubmitBuilder SetSendTime(string sendTimeString)
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
        /// <returns>ResubmitBuilder</returns>
        public ResubmitBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        /// <summary>
        /// Build ResubmitRequestOptions
        /// </summary>
        /// <returns>ResubmitRequestOptions</returns>
        public ResubmitRequestOptions Build()
        {
            return Entity;
        }

        /// <summary>
        /// Build ResubmitRequestOptions (async)
        /// </summary>
        /// <returns>Task<ResubmitRequestOptions></returns>
        public async Task<ResubmitRequestOptions> BuildAsync()
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
