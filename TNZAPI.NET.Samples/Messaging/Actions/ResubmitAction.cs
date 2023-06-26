using TNZAPI.NET.Api.Actions.Resubmit.Dto;
using TNZAPI.NET.Api.Messaging.Common;
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

        public void Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Resubmit.Submit("ID123456"); // Message ID

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
        }

        public void Simple() => Basic();

        public void Builder() => Basic();

        public void Advanced()
        {
            //
            // Sample code using ResubmitBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Resubmit.Submit(new ResubmitRequestOptions()
            {
                MessageID = "ID123456",                     // MessageID
                SendTime = new DateTime().AddMinutes(5)     // Optional: Set SendTime
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
        }
    }
}
