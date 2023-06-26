using TNZAPI.NET.Api.Actions.Abort.Dto;

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
                MessageID = messageID
            };
        }

        /// <summary>
        /// Set message id
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>AbortBuilder</returns>
        public AbortBuilder SetMessageID(string messageID)
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
