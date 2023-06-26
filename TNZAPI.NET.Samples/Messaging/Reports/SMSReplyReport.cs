using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Reports.SMSReply;
using TNZAPI.NET.Api.Reports.SMSReply.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Reports
{
    public class SMSReplyReport
    {
        private readonly ITNZAuth apiUser;

        public SMSReplyReport(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public SMSReplyApiResult Basic(string? messageID = null)
        {
            var client = new TNZApiClient(apiUser);

            if (messageID is null)
            {
                messageID = "ID123456";
            }

            var response = client.Reports.SMSReply.Poll(messageID);

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
                    Console.WriteLine($"    -> MessageText: '{message.MessageText}'");
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

                    foreach (var reply in message.SMSReplies)
                    {
                        Console.WriteLine($"======================================");
                        Console.WriteLine($" => SMS Reply");
                        Console.WriteLine($"    -> Date: '{reply.Date}'");
                        Console.WriteLine($"    -> DateUTC: '{reply.DateUTC}'");
                        Console.WriteLine($"    -> From: '{reply.From}'");
                        Console.WriteLine($"    -> MessageText: '{reply.MessageText}'");
                    }
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

        public SMSReplyApiResult Simple(string? messageID = null) => Basic(messageID);        // Same as Basic

        public SMSReplyApiResult Builder(SMSReplyRequestOptions? options = null)
        {
            var client = new TNZApiClient(apiUser);
            
            if (options is null)
            {
                options = new SMSReplyBuilder("ID123456")
                                .SetRecordsPerPage(50)
                                .SetPage(1)
                                .Build();
            }

            var response = client.Reports.SMSReply.Poll(options);

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
                    Console.WriteLine($"    -> MessageText: '{message.MessageText}'");
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

                    foreach (var reply in message.SMSReplies)
                    {
                        Console.WriteLine($"======================================");
                        Console.WriteLine($" => SMS Reply");
                        Console.WriteLine($"    -> Date: '{reply.Date}'");
                        Console.WriteLine($"    -> DateUTC: '{reply.DateUTC}'");
                        Console.WriteLine($"    -> From: '{reply.From}'");
                        Console.WriteLine($"    -> MessageText: '{reply.MessageText}'");
                    }
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

        public void Advanced(SMSReplyRequestOptions? options = null)
        {
            var client = new TNZApiClient(apiUser);

            if (options is null)
            {
                options = new SMSReplyRequestOptions()
                {
                    MessageID = "ID123456",
                    RecordsPerPage = 50,
                    Page = 1
                };
            }

            var response = client.Reports.SMSReply.Poll(options);

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
                    Console.WriteLine($"    -> MessageText: '{message.MessageText}'");
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

                    foreach (var reply in message.SMSReplies)
                    {
                        Console.WriteLine($"======================================");
                        Console.WriteLine($" => SMS Reply");
                        Console.WriteLine($"    -> Date: '{reply.Date}'");
                        Console.WriteLine($"    -> DateUTC: '{reply.DateUTC}'");
                        Console.WriteLine($"    -> From: '{reply.From}'");
                        Console.WriteLine($"    -> MessageText: '{reply.MessageText}'");
                    }
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
