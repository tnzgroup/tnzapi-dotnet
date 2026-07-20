using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.SMS
{
    /// <summary>
    /// Reference code demonstrating client.Messaging.SMS and client.Actions.SMS.
    /// This class is not a runnable program — call these methods from your own application.
    /// Full parameter reference: docs/sms.md.
    /// </summary>
    public class SMSSamples
    {
        private readonly ITNZAuth apiUser;

        public SMSSamples()
        {
            apiUser = new TNZApiUser();
        }

        public SMSSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public SMSApiResult Send()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.SMS.SendMessage(
                toNumber: "+64211111111",
                messageText: "Test SMS",
                sendMode: Enums.SendModeType.Test          // TEST Mode - remove this to send live traffic
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
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

        public SMSApiResult SendUsingBuilder()
        {
            var client = new TNZApiClient(apiUser);

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");
            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            using var builder = new SMSBuilder();

            var model = builder
                .SetMessageText("Test SMS")
                .SetReference("Test SMS - Builder sample")
                .AddDestination("+64211111111")
                .AddDestination("+64222222222")
                .AddDestination(contactID)                  // Destination from TNZ Addressbook by ContactID
                .AddDestinations(groupID)                   // Destinations from TNZ Addressbook by GroupID
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.SMS.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
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

        public SMSApiResult SendUsingDestination()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new SMSBuilder();

            var model = builder
                .SetMessageText("Hi [[FirstName]], your appointment is on [[Custom1]].")
                .AddDestination(new Destination { Recipient = "+64211111111", FirstName = "Alice", Custom1 = "Monday 3pm" })
                .AddDestination("+64222222222")             // plain string also works — sets the primary Recipient field
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.SMS.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public SMSApiResult SendToMultipleRecipients()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new SMSBuilder();

            var model = builder
                .SetMessageText("Hi [[FirstName]], your appointment is on [[Custom1]].")
                .AddDestination(new Destination { Recipient = "+64211111111", FirstName = "Alice", Custom1 = "Monday 3pm" })
                .AddDestination(new Destination { Recipient = "+64222222222", FirstName = "Bob", Custom1 = "Tuesday 10am" })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.SMS.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public SMSApiResult SendScheduledWithWebhook()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new SMSBuilder();

            var model = builder
                .SetMessageText("Your reminder.")
                .AddDestination("+64211111111")
                .SetSendTime(DateTime.Now.AddDays(1))
                .SetTimezone("New Zealand")
                .SetWebhookCallbackURL("https://yourapp.example.com/webhooks/sms")
                .SetWebhookCallbackFormat(Enums.WebhookCallbackType.JSON)
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.SMS.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public SMSStatusApiResult Status(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.SMS.Status(messageID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Status of MessageID '{response.MessageID}':");
                Console.WriteLine($" => JobStatus: '{response.JobStatus}'");
                Console.WriteLine($" => JobNum: '{response.JobNum}'");
                Console.WriteLine($" => Count: {response.Count}, Complete: {response.Complete}, Success: {response.Success}, Failed: {response.Failed}");

                foreach (var recipient in response.Recipients)
                {
                    Console.WriteLine($"    -> {recipient.Destination}: {recipient.Status} ({recipient.Result})");

                    foreach (var reply in recipient.SMSReplies)
                    {
                        Console.WriteLine($"       reply: '{reply.MessageText}'");
                    }
                }
            }
            else
            {
                foreach (var error in response.ErrorMessage)
                {
                    Console.WriteLine($"- Error={error}");
                }
            }

            return response;
        }

        public SMSReceivedApiResult Received()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.SMS.Received(
                timePeriod: 1440,       // Minutes to look back
                recordsPerPage: 100,
                page: 1
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var message in response.Messages)
                {
                    Console.WriteLine($" => From: '{message.From}', MessageText: '{message.MessageText}'");
                }
            }

            return response;
        }

        public SMSReceivedApiResult ReceivedByDateRange()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.SMS.Received(
                dateFrom: DateTime.Now.AddDays(-1),
                dateTo: DateTime.Now,          // optional — omit to default to now
                recordsPerPage: 100,
                page: 1
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var message in response.Messages)
                {
                    Console.WriteLine($" => From: '{message.From}', MessageText: '{message.MessageText}'");
                }
            }

            return response;
        }

        public SMSStatusApiResult SMSReply(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.SMS.SMSReply(messageID, recordsPerPage: 100, page: 1);

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var recipient in response.Recipients)
                {
                    foreach (var reply in recipient.SMSReplies)
                    {
                        Console.WriteLine($" => {recipient.Destination} replied: '{reply.MessageText}'");
                    }
                }
            }

            return response;
        }

        public SMSActionApiResult Reschedule(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Actions.SMS.Reschedule(messageID, DateTime.Now.AddHours(1));

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Action: '{response.Action}', Status: '{response.Status}'");
            }

            return response;
        }

        public SMSActionApiResult Abort(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Actions.SMS.Abort(messageID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Action: '{response.Action}', Status: '{response.Status}'");
            }

            return response;
        }
    }
}