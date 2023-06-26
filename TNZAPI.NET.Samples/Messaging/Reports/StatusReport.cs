using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Reports.Status.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Reports
{
    public class StatusReport
    {
        private readonly ITNZAuth apiUser;

        public StatusReport(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public void Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Reports.Status.Poll("ID123456");

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Status of MessageID '{response.MessageID}':");
                Console.WriteLine($" => Status: '{response.Status}'");
                Console.WriteLine($" => JobNum: '{response.JobNum}'");
                Console.WriteLine($" => Account: '{response.Account}'");
                Console.WriteLine($" => SubAccount: '{response.SubAccount}'");
                Console.WriteLine($" => Department: '{response.Department}'");
                Console.WriteLine($" => Reference: '{response.Reference}'");
                Console.WriteLine($" => Created: '{response.Created}'");
                Console.WriteLine($" => CreatedUTC: '{response.CreatedUTC}'");
                Console.WriteLine($" => Delayed: '{response.Delayed}'");
                Console.WriteLine($" => DelayedUTC: '{response.DelayedUTC}'");
                Console.WriteLine($" => Count: {response.Count}");
                Console.WriteLine($" => Complete: {response.Complete}");
                Console.WriteLine($" => Success: {response.Success}");
                Console.WriteLine($" => Failed: {response.Failed}");
                Console.WriteLine($" => TotalRecords (Recipients): {response.TotalRecords}");
                Console.WriteLine($" => PageCount (Recipients): {response.PageCount}");
                Console.WriteLine($" => RecordsPerPage (Recipients): {response.RecordsPerPage}");
                Console.WriteLine($" => Page (Recipients): {response.Page}");

                foreach (var message in response.Recipients)
                {
                    Console.WriteLine($"======================================");
                    Console.WriteLine($" => Message Delivered");
                    Console.WriteLine($"    -> Type: '{message.Type}'");
                    Console.WriteLine($"    -> DestSeq: '{message.DestSeq}'");
                    Console.WriteLine($"    -> Destination: '{message.Destination}'");
                    Console.WriteLine($"    -> Status: '{message.Status}'");
                    Console.WriteLine($"    -> Result: '{message.Result}'");
                    Console.WriteLine($"    -> SentDate: '{message.SentDate}'");
                    Console.WriteLine($"    -> Attention: '{message.Attention}'");
                    Console.WriteLine($"    -> Company: '{message.Company}'");
                    Console.WriteLine($"    -> Custom1: '{message.Custom1}'");
                    Console.WriteLine($"    -> Custom2: '{message.Custom2}'");
                    Console.WriteLine($"    -> Custom3: '{message.Custom3}'");
                    Console.WriteLine($"    -> Custom4: '{message.Custom4}'");
                    Console.WriteLine($"    -> Custom5: '{message.Custom5}'");
                    Console.WriteLine($"    -> Custom6: '{message.Custom6}'");
                    Console.WriteLine($"    -> Custom7: '{message.Custom7}'");
                    Console.WriteLine($"    -> Custom8: '{message.Custom8}'");
                    Console.WriteLine($"    -> Custom9: '{message.Custom9}'");
                    Console.WriteLine($"    -> RemoteID: '{message.RemoteID}'");
                    Console.WriteLine($"    -> Price: '{message.Price}'");
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
        }

        public void Simple() => Basic();        // Same as Basic

        public void Builder() => Basic();       // Don't have one

        public void Advanced()
        {
            // Sample code use StatusRequestBuilder()

            var client = new TNZApiClient(apiUser);

            var options = new StatusRequestOptions()
            {
                MessageID = "ID123456"
            };

            var response = client.Reports.Status.Poll(options);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Status of MessageID '{response.MessageID}':");
                Console.WriteLine($" => Status: '{response.Status}'");
                Console.WriteLine($" => JobNum: '{response.JobNum}'");
                Console.WriteLine($" => Account: '{response.Account}'");
                Console.WriteLine($" => SubAccount: '{response.SubAccount}'");
                Console.WriteLine($" => Department: '{response.Department}'");
                Console.WriteLine($" => Reference: '{response.Reference}'");
                Console.WriteLine($" => Created: '{response.Created}'");
                Console.WriteLine($" => CreatedUTC: '{response.CreatedUTC}'");
                Console.WriteLine($" => Delayed: '{response.Delayed}'");
                Console.WriteLine($" => DelayedUTC: '{response.DelayedUTC}'");
                Console.WriteLine($" => Count: {response.Count}");
                Console.WriteLine($" => Complete: {response.Complete}");
                Console.WriteLine($" => Success: {response.Success}");
                Console.WriteLine($" => Failed: {response.Failed}");
                Console.WriteLine($" => TotalRecords (Recipients): {response.TotalRecords}");
                Console.WriteLine($" => PageCount (Recipients): {response.PageCount}");
                Console.WriteLine($" => RecordsPerPage (Recipients): {response.RecordsPerPage}");
                Console.WriteLine($" => Page (Recipients): {response.Page}");

                foreach (var message in response.Recipients)
                {
                    Console.WriteLine($"======================================");
                    Console.WriteLine($" => Message Delivered");
                    Console.WriteLine($"    -> Type: '{message.Type}'");
                    Console.WriteLine($"    -> DestSeq: '{message.DestSeq}'");
                    Console.WriteLine($"    -> Destination: '{message.Destination}'");
                    Console.WriteLine($"    -> Status: '{message.Status}'");
                    Console.WriteLine($"    -> Result: '{message.Result}'");
                    Console.WriteLine($"    -> SentDate: '{message.SentDate}'");
                    Console.WriteLine($"    -> Attention: '{message.Attention}'");
                    Console.WriteLine($"    -> Company: '{message.Company}'");
                    Console.WriteLine($"    -> Custom1: '{message.Custom1}'");
                    Console.WriteLine($"    -> Custom2: '{message.Custom2}'");
                    Console.WriteLine($"    -> Custom3: '{message.Custom3}'");
                    Console.WriteLine($"    -> Custom4: '{message.Custom4}'");
                    Console.WriteLine($"    -> Custom5: '{message.Custom5}'");
                    Console.WriteLine($"    -> Custom6: '{message.Custom6}'");
                    Console.WriteLine($"    -> Custom7: '{message.Custom7}'");
                    Console.WriteLine($"    -> Custom8: '{message.Custom8}'");
                    Console.WriteLine($"    -> Custom9: '{message.Custom9}'");
                    Console.WriteLine($"    -> RemoteID: '{message.RemoteID}'");
                    Console.WriteLine($"    -> Price: '{message.Price}'");
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
        }
    }
}
