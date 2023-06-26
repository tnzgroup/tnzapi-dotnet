using TNZAPI.NET.Api.Reports.SMSReceived.Dto;

namespace TNZAPI.NET.Api.Reports.SMSReceived
{
    public class SMSReceivedBuilder : IDisposable
    {
        private SMSReceivedRequestOptions Entity { get; set; }

        public SMSReceivedBuilder()
        {
            Entity = new SMSReceivedRequestOptions();
        }

        public void Dispose()
        {
            Entity = null;
        }

        /// <summary>
        /// Sets TimePeriod
        /// </summary>
        /// <param name="timePeriod">Minutes</param>
        /// <returns>SMSReceivedBuilder</returns>
        public SMSReceivedBuilder SetPeriod(int timePeriod)
        {
            Entity.TimePeriod = timePeriod;

            return this;
        }

        /// <summary>
        /// Sets DateFrom
        /// </summary>
        /// <param name="dateFrom">DateTime</param>
        /// <returns>SMSReceivedBuilder</returns>
        public SMSReceivedBuilder SetDateFrom(DateTime dateFrom)
        {
            Entity.DateFrom = dateFrom;

            return this;
        }

        /// <summary>
        /// Sets DateFrom by using DateTime.TryParse() function
        /// </summary>
        /// <param name="dateFromString">DateTime string</param>
        /// <returns>SMSReceivedBuilder</returns>
        public SMSReceivedBuilder SetDateFrom(string dateFromString)
        {
            DateTime dateFrom;

            DateTime.TryParse(dateFromString, out dateFrom);

            Entity.DateFrom = dateFrom;

            return this;
        }

        /// <summary>
        /// Sets DateTo
        /// </summary>
        /// <param name="dateTo">DateTime</param>
        /// <returns>SMSReceivedBuilder</returns>
        public SMSReceivedBuilder SetDateTo(DateTime dateTo)
        {
            Entity.DateTo = dateTo;

            return this;
        }

        /// <summary>
        /// Sets DateTo by using DateTime.TryParse() function
        /// </summary>
        /// <param name="dateToString">DateTime string</param>
        /// <returns>SMSReceivedBuilder</returns>
        public SMSReceivedBuilder SetDateTo(string dateToString)
        {
            DateTime dateTo;

            DateTime.TryParse(dateToString, out dateTo);

            Entity.DateFrom = dateTo;

            return this;
        }

        /// <summary>
        /// Sets no. of records per page
        /// </summary>
        /// <param name="recordsPerPage">Records per page</param>
        /// <returns>SMSReceivedBuilder</returns>
        public SMSReceivedBuilder SetRecordsPerPage(int recordsPerPage)
        {
            Entity.RecordsPerPage = recordsPerPage;

            return this;
        }

        /// <summary>
        /// Sets page
        /// </summary>
        /// <param name="page">Current location</param>
        /// <returns>SMSReceivedBuilder</returns>
        public SMSReceivedBuilder SetPage(int page)
        {
            Entity.Page = page;

            return this;
        }


        /// <summary>
        /// Build SMSReceivedRequestOptions
        /// </summary>
        /// <returns>SMSReceivedRequestOptions</returns>
        public SMSReceivedRequestOptions Build()
        {
            return Entity;
        }

        /// <summary>
        /// Build SMSReceivedRequestOptions (async)
        /// </summary>
        /// <returns>Task<SMSReceivedRequestOptions></returns>
        public async Task<SMSReceivedRequestOptions> BuildAsync()
        {
            return await Task.Run(() =>
            {
                return Entity;
            });
        }
    }
}
