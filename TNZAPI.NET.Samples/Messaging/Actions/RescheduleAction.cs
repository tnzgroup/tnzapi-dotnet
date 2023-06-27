using TNZAPI.NET.Api.Actions.Reschedule;
using TNZAPI.NET.Api.Actions.Reschedule.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Actions
{
    public class RescheduleAction
    {
        private readonly ITNZAuth apiUser;

        public RescheduleAction(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public RescheduleApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Reschedule.Submit(
                messageID: "ID123456",                              // MessageID
                sendTime: DateTime.Parse("2023-12-31T12:00:00")     // Set send time
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Status of MessageID '" + response.MessageID + "':");
                Console.WriteLine(" => Status: '" + response.GetStatusString() + "'");
                Console.WriteLine(" => JobNum: '" + response.JobNum + "'");
                Console.WriteLine(" => Action: '" + response.Action + "'");
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

        public RescheduleApiResult Simple() => Basic();        // Same as Basic

        public RescheduleApiResult Builder()
        {
            var client = new TNZApiClient(apiUser);

            var options = new RescheduleBuilder("ID123456")                             // MessageID
                                .SetSendTime(DateTime.Parse("2023-12-31T12:00:00"))     // Set send time
                                .SetTimezone("New Zealand")                             // Timezone (optional)
                                .Build();

            var response = client.Actions.Reschedule.Submit(options);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Status of MessageID '" + response.MessageID + "':");
                Console.WriteLine(" => Status: '" + response.GetStatusString() + "'");
                Console.WriteLine(" => JobNum: '" + response.JobNum + "'");
                Console.WriteLine(" => Action: '" + response.Action + "'");
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

        public RescheduleApiResult Advanced()
        {
            //
            // Sample code using RescheduleBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Reschedule.Submit(
                new RescheduleRequestOptions()
                {
                    MessageID = "ID123456",                             // MessageID
                    SendTime = DateTime.Parse("2023-12-31T12:00:00"),   // Set send time
                    Timezone = "New Zealand"                            // Timezone (optional)
                });

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Status of MessageID '" + response.MessageID + "':");
                Console.WriteLine(" => Status: '" + response.GetStatusString() + "'");
                Console.WriteLine(" => JobNum: '" + response.JobNum + "'");
                Console.WriteLine(" => Action: '" + response.Action + "'");
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
