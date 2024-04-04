using TNZAPI.NET.Api.Actions.Reschedule;
using TNZAPI.NET.Api.Actions.Resubmit.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Actions
{
    public class ResubmitAction
    {
        private readonly ITNZAuth apiUser;

        public ResubmitAction(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public ResubmitApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Resubmit.Submit(new MessageID("ID123456")); // Message ID

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

        public ResubmitApiResult Simple() => Basic();

        public ResubmitApiResult Builder()
        {
            var client = new TNZApiClient(apiUser);

            var options = new ResubmitBuilder(new MessageID("ID123456"))    // MessageID
                            .SetSendTime(new DateTime().AddMinutes(5))      // Optional: Set SendTime
                            .SetTimezone("New Zealand")                     // Optional: Set Timezone
                            .Build();

            var response = client.Actions.Resubmit.Submit(options);

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

        public ResubmitApiResult Advanced()
        {
            //
            // Sample code using ResubmitBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Resubmit.Submit(new ResubmitRequestOptions()
            {
                MessageID = new MessageID("ID123456"),      // MessageID
                SendTime = new DateTime().AddMinutes(5),    // Optional: Set SendTime
                Timezone = "New Zealand"                    // Optional: Set Timezone
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
