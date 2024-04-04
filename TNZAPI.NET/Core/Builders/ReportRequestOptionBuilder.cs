using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core.Interfaces.Reports;

namespace TNZAPI.NET.Core.Builders
{
    public class ReportRequestOptionBuilder<T> where T: class, IReportRequestOptions, new()
    {
        public T Entity { get; set; }

        public ReportRequestOptionBuilder()
        {
            Entity = new T();
        }

        /// <summary>
        /// Construct with MessageID
        /// </summary>
        /// <param name="messageID">MessageID</param>
        public ReportRequestOptionBuilder(string messageID)
        {
            Entity = new T()
            {
                MessageID = new MessageID(messageID)
            };
        }

		/// <summary>
		/// Construct with MessageID
		/// </summary>
		/// <param name="messageID">MessageID</param>
		public ReportRequestOptionBuilder(MessageID messageID)
		{
			Entity = new T()
			{
				MessageID = messageID
			};
		}

		/// <summary>
		/// Set Message ID
		/// </summary>
		/// <param name="messageID">MessageID</param>
		/// <returns>ReportRequestOptionBuilder<T></returns>
		public ReportRequestOptionBuilder<T> SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

		/// <summary>
		/// Set Message ID
		/// </summary>
		/// <param name="messageID">MessageID</param>
		/// <returns>ReportRequestOptionBuilder<T></returns>
		public ReportRequestOptionBuilder<T> SetMessageID(MessageID messageID)
		{
			Entity.MessageID = messageID;

			return this;
		}

		/// <summary>
		/// Set no. of records per page
		/// </summary>
		/// <param name="recordsPerPage">Records per page</param>
		/// <returns>ReportRequestOptionBuilder<T></returns>
		public ReportRequestOptionBuilder<T> SetRecordsPerPage(int recordsPerPage)
        {
            Entity.RecordsPerPage = recordsPerPage;

            return this;
        }

        /// <summary>
        /// Set page
        /// </summary>
        /// <param name="page">Current Location</param>
        /// <returns>ReportRequestOptionBuilder<T></returns>
        public ReportRequestOptionBuilder<T> SetPage(int page)
        {
            Entity.Page = page;

            return this;
        }

        /// <summary>
        /// Build object
        /// </summary>
        /// <returns>T</returns>
        public T Build()
        {
            return Entity;
        }

        /// <summary>
        /// Build object (async)
        /// </summary>
        /// <returns>Task<T></returns>
        public async Task<T> BuildAsync()
        {
            return await Task.Run(() =>
            {
                return Entity;
            });
        }
    }
}
