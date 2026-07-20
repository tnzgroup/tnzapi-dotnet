using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.RCS;
using TNZAPI.NET.Api.Messaging.RCS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.RCS
{
    /// <summary>
    /// Reference code demonstrating client.Messaging.RCS and client.Actions.RCS.
    /// This class is not a runnable program — call these methods from your own application.
    /// Full parameter reference: docs/rcs.md.
    /// </summary>
    public class RCSSamples
    {
        private readonly ITNZAuth apiUser;

        public RCSSamples()
        {
            apiUser = new TNZApiUser();
        }

        public RCSSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public RCSApiResult Send()
        {
            // Unlike WhatsApp, RCS's Message/TemplateID are either/or — like SMS.
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.RCS.SendMessage(
                messageText: "Test RCS message",
                toNumber: "+64211111111",
                sendMode: Enums.SendModeType.Test
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
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

        public RCSApiResult SendUsingBuilder()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new RCSBuilder();

            var model = builder
                .SetMessageText("Test RCS message")
                .SetFromNumber("61410023004")               // Sender ID, E.164 without leading '+'
                .AddDestination("+64211111111")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.RCS.SendMessage(model);
        }

        public RCSApiResult SendUsingDestination()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new RCSBuilder();

            var model = builder
                .SetMessageText("Test RCS message")
                .SetFromNumber("61410023004")               // Sender ID, E.164 without leading '+'
                .AddDestination(new Destination { Recipient = "+64211111111", FirstName = "Alice" })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.RCS.SendMessage(model);
        }

        public RCSApiResult SendToMultipleRecipients()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new RCSBuilder();

            var model = builder
                .SetMessageText("Test RCS message")
                .AddDestination("+64211111111")
                .AddDestination("+64222222222")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.RCS.SendMessage(model);
        }

        public RCSApiResult SendScheduled()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new RCSBuilder();

            var model = builder
                .SetMessageText("Your reminder.")
                .AddDestination("+64211111111")
                .SetSendTime(DateTime.Now.AddDays(1))
                .SetTimezone("New Zealand")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.RCS.SendMessage(model);
        }

        public RCSApiResult SendWithAttachment()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new RCSBuilder();

            var model = builder
                .SetMessageText("Here's your invoice.")
                .AddDestination("+64211111111")
                .AddAttachment("Invoice.pdf", "[base64-encoded-file-data]")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.RCS.SendMessage(model);
        }

        public RCSStatusApiResult Status(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.RCS.Status(messageID);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"JobStatus: '{response.JobStatus}', JobNum: '{response.JobNum}'");

                foreach (var recipient in response.Recipients)
                {
                    Console.WriteLine($" -> {recipient.Destination}: {recipient.Status} ({recipient.Result})");
                }
            }

            return response;
        }

        public RCSReceivedApiResult Received()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.RCS.Received(timePeriod: 1440);

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var message in response.Messages)
                {
                    Console.WriteLine($" => From: '{message.From}', MessageText: '{message.MessageText}'");
                }
            }

            return response;
        }

        public RCSReceivedApiResult ReceivedByDateRange()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.RCS.Received(dateFrom: DateTime.Now.AddDays(-1), recordsPerPage: 100, page: 1);

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var message in response.Messages)
                {
                    Console.WriteLine($" => From: '{message.From}', MessageText: '{message.MessageText}'");
                }
            }

            return response;
        }

        public RCSActionApiResult Reschedule(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.RCS.Reschedule(messageID, DateTime.Now.AddHours(1));
        }

        public RCSActionApiResult Abort(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.RCS.Abort(messageID);
        }
    }
}