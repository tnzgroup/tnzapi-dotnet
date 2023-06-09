using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Reports.SMSReceived.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Reports
{
    public class SMSReceivedReport
    {
        private readonly ITNZAuth apiUser;

        public SMSReceivedReport(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public void Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Reports.SMSReceived.List(
                timePeriod: 1440,       // No. of minutes
                recordsPerPage: 100,    // x numbers of records to return per request
                page: 1                 // current location
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var received in response.Messages)
                {
                    Console.WriteLine("======================================");
                    Console.WriteLine(" => MessageReceived");
                    Console.WriteLine("    -> Date: '" + received.Date.ToString("yyyy-MM-dd hh:mm:ss") + "'");
                    Console.WriteLine("    -> From: '" + received.From + "'");
                    Console.WriteLine("    -> MessageText: '" + received.MessageText.Replace("'", "\'") + "'");
                }

            }
            else
            {
                Console.WriteLine("Error occurred while processing...");
                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- {error}");
                }
            }
        }

        public void Simple()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Reports.SMSReceived.List(
                timePeriod: 1440    // No. of minutes
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var received in response.Messages)
                {
                    Console.WriteLine("======================================");
                    Console.WriteLine(" => MessageReceived");
                    Console.WriteLine("    -> Date: '" + received.Date.ToString("yyyy-MM-dd hh:mm:ss") + "'");
                    Console.WriteLine("    -> From: '" + received.From + "'");
                    Console.WriteLine("    -> MessageText: '" + received.MessageText.Replace("'", "\'") + "'");
                }

            }
            else
            {
                Console.WriteLine("Error occurred while processing...");
                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- {error}");
                }
            }
        }

        public void Builder() => Basic();                       // Builder NOT supported

        public void Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Reports.SMSReceived.List(
                new SMSReceivedRequestOptions()
                {
                    DateFrom = DateTime.Parse("2023-08-01T00:00:00"),       // Return results from the date/time
                    DateTo = DateTime.Parse("2023-08-01T23:59:59"),         // Return results to the date/time
                    RecordsPerPage = 10,                                    // x numbers of records to return per request
                    Page = 1                                                // current location
                });

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Inbound SMS Messages:");

                foreach (var received in response.Messages)
                {
                    Console.WriteLine("======================================");
                    Console.WriteLine(" => MessageReceived");
                    Console.WriteLine("    -> Date: '" + received.Date.ToString("yyyy-MM-dd hh:mm:ss") + "'");
                    Console.WriteLine("    -> From: '" + received.From + "'");
                    Console.WriteLine("    -> MessageText: '" + received.MessageText.Replace("'", "\'") + "'");
                }

            }
            else
            {
                Console.WriteLine("Error occurred while processing...");
                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- {error}");
                }
            }
        }
    }
}
