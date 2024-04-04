using TNZAPI.NET.Api.Actions.Pacing.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Actions.Pacing
{
    public class PacingBuilder : IDisposable
    {
        private PacingRequestOptions Entity { get; set; }

        public PacingBuilder()
        {
            Entity = new PacingRequestOptions();
        }

        public PacingBuilder(string messageID)
        {
            Entity = new PacingRequestOptions()
            {
                MessageID = new MessageID(messageID)
            };
        }

        public PacingBuilder(MessageID messageID)
        {
            Entity = new PacingRequestOptions()
            {
                MessageID = messageID
            };
        }

        /// <summary>
        /// Set Message ID
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>PacingBuilder</returns>
        public PacingBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);
            
            return this;
        }

        /// <summary>
        /// Set number of operators - No. of concurrent calls
        /// </summary>
        /// <param name="numberOfOperators">no of operators</param>
        /// <returns>PacingBuilder</returns>
        public PacingBuilder SetNumberOfOperators(int numberOfOperators)
        {
            Entity.NumberOfOperators = numberOfOperators;

            return this;
        }

        /// <summary>
        /// Build PacingRequestOptions
        /// </summary>
        /// <returns>PacingRequestOptions</returns>
        public PacingRequestOptions Build()
        {
            return Entity;
        }

        /// <summary>
        /// Build PacingRequestOptions (async)
        /// </summary>
        /// <returns>Task<PacingRequestOptions></returns>
        public async Task<PacingRequestOptions> BuildAsync()
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
