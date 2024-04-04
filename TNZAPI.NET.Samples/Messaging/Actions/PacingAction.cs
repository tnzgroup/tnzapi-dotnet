using TNZAPI.NET.Api.Actions.Pacing;
using TNZAPI.NET.Api.Actions.Pacing.Dto;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Actions
{
    public class PacingAction
    {
        private readonly ITNZAuth apiUser;

        public PacingAction(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public PacingApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Pacing.Submit(
                messageID: new MessageID("ID123456"),   // MessageID
                numberOfOperators: 1                    // No. of operators
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

        public void Simple() => Basic();        // Same as Basic()

        public void Builder() => Basic();       // Doesn't support Builder()

        public PacingApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Pacing.Submit(
                new PacingRequestOptions()
                {
                    MessageID = new MessageID("ID123456"),     // MessageID
                    NumberOfOperators = 1,      // No. of operators
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
