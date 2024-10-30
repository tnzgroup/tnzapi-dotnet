using TNZAPI.NET.Api.Reports.SMSReceived;
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

        public SMSReceivedApiResult Basic()
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
                    Console.WriteLine($"======================================");
                    Console.WriteLine($" => MessageReceived");
										Console.WriteLine($"    -> ReceivedID: '{received.ReceivedID}'");
										Console.WriteLine($"    -> MessageID: '{received.MessageID}'");
										Console.WriteLine($"    -> ContactID: '{received.ContactID}'");
										Console.WriteLine($"    -> Date: '{received.Date.ToString("yyyy-MM-dd hh:mm:ss")}'");
                    Console.WriteLine($"    -> From: '{received.From}'");
                    Console.WriteLine($"    -> MessageText: '{received.MessageText.Replace("'", "\'")}'");
                }

            }
            else
            {
                Console.WriteLine("Error occurred while processing.");

                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }

        public SMSReceivedApiResult Simple()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Reports.SMSReceived.List(
                dateFrom: DateTime.Parse("2023-08-01T00:00:00"),    // Return results from the date/time
                dateTo: DateTime.Parse("2023-08-01T23:59:59"),      // Return results from the date/time
                page: 1                                             // current location
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var received in response.Messages)
                {
                    Console.WriteLine($"======================================");
                    Console.WriteLine($" => MessageReceived");
										Console.WriteLine($"    -> ReceivedID: '{received.ReceivedID}'");
										Console.WriteLine($"    -> MessageID: '{received.MessageID}'");
										Console.WriteLine($"    -> ContactID: '{received.ContactID}'");
										Console.WriteLine($"    -> Date: '{received.Date.ToString("yyyy-MM-dd hh:mm:ss")}'");
                    Console.WriteLine($"    -> From: '{received.From}'");
                    Console.WriteLine($"    -> MessageText: '{received.MessageText.Replace("'", "\'")}'");
                }

            }
            else
            {
                Console.WriteLine("Error occurred while processing.");

                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }

        public SMSReceivedApiResult Builder(SMSReceivedRequestOptions? options = null)
        {
            var client = new TNZApiClient(apiUser);

            if (options is null)
            {
                options = new SMSReceivedBuilder()
                            .SetDateFrom("2023-08-01T00:00:00")             // Return results from the date/time
                            .SetDateTo("2023-08-01T23:59:59")               // Return results to the date/time
                            .SetRecordsPerPage(10)                          // x numbers of records to return per request
                            .SetPage(1)                                     // current location
                            .Build();
            }

            var response = client.Reports.SMSReceived.List(options);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Inbound SMS Messages:");

                foreach (var received in response.Messages)
                {
                    Console.WriteLine($"======================================");
                    Console.WriteLine($" => MessageReceived");
										Console.WriteLine($"    -> ReceivedID: '{received.ReceivedID}'");
										Console.WriteLine($"    -> MessageID: '{received.MessageID}'");
										Console.WriteLine($"    -> ContactID: '{received.ContactID}'");
										Console.WriteLine($"    -> Date: '{received.Date.ToString("yyyy-MM-dd hh:mm:ss")}'");
                    Console.WriteLine($"    -> From: '{received.From}'");
                    Console.WriteLine($"    -> MessageText: '{received.MessageText.Replace("'", "\'")}'");
                }

            }
            else
            {
                Console.WriteLine("Error occurred while processing.");

                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }

        public SMSReceivedApiResult Advanced(SMSReceivedRequestOptions? options = null)
        {
            var client = new TNZApiClient(apiUser);

            if (options is null)
            {
                options = new SMSReceivedRequestOptions()
                {
                    DateFrom = DateTime.Parse("2023-08-01T00:00:00"),       // Return results from the date/time
                    DateTo = DateTime.Parse("2023-08-01T23:59:59"),         // Return results to the date/time
                    RecordsPerPage = 10,                                    // x numbers of records to return per request
                    Page = 1                                                // current location
                };
            }

            var response = client.Reports.SMSReceived.List(options);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Inbound SMS Messages:");

                foreach (var received in response.Messages)
                {
                    Console.WriteLine($"======================================");
                    Console.WriteLine($" => MessageReceived");
										Console.WriteLine($"    -> ReceivedID: '{received.ReceivedID}'");
										Console.WriteLine($"    -> MessageID: '{received.MessageID}'");
										Console.WriteLine($"    -> ContactID: '{received.ContactID}'");
										Console.WriteLine($"    -> Date: '{received.Date.ToString("yyyy-MM-dd hh:mm:ss")}'");
                    Console.WriteLine($"    -> From: '{received.From}'");
                    Console.WriteLine($"    -> MessageText: '{received.MessageText.Replace("'", "\'")}'");
                }
            }
            else
            {
                Console.WriteLine("Error occurred while processing.");

                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }
    }
}
