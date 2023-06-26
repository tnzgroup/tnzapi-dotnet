using TNZAPI.NET.Api.Actions.Abort.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Actions
{
    public class AbortAction
    {
        private readonly ITNZAuth apiUser;

        public AbortAction(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public AbortApiResult Basic()
        {
            //
            // AuthToken can be found from our web portal
            //

            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Abort.Submit("ID123456"); // MessageID

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

        public AbortApiResult Simple() => Basic();        // Same as Basic()

        public AbortApiResult Builder() => Basic();

        public AbortApiResult Advanced()
        {
            //
            // Sample code using AbortBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Abort.Submit(
                new AbortRequestOptions()
                {
                    MessageID = "ID123456"      // MessageID
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
