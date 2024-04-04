using TNZAPI.NET.Api.Actions.Abort.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Actions.Abort
{
    public class AbortBuilder : IDisposable
    {
        private AbortRequestOptions Entity { get; set; }

        public AbortBuilder()
        {
            Entity = new AbortRequestOptions();
        }

        public AbortBuilder(string messageID)
        {
            Entity = new AbortRequestOptions()
            {
                MessageID = new MessageID(messageID)
            };
        }

        public AbortBuilder(MessageID messageID)
        {
			Entity = new AbortRequestOptions()
			{
				MessageID = messageID
			};
		}

        /// <summary>
        /// Sets message id
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>AbortBuilder</returns>
        public AbortBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

		/// <summary>
		/// Sets message id
		/// </summary>
		/// <param name="messageID">MessageID</param>
		/// <returns>AbortBuilder</returns>
		public AbortBuilder SetMessageID(MessageID messageID)
		{
			Entity.MessageID = messageID;

			return this;
		}

		/// <summary>
		/// Build AbortRequestOptions
		/// </summary>
		/// <returns>AbortRequestOptions</returns>
		public AbortRequestOptions Build()
        {
            return Entity;
        }

        /// <summary>
        /// Build AbortRequestOptions
        /// </summary>
        /// <returns>Task<AbortRequestOptions></returns>
        public async Task<AbortRequestOptions> BuildAsync()
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
