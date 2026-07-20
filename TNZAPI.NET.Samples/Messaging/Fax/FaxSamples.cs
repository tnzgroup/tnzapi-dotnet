using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Fax
{
    /// <summary>
    /// Reference code demonstrating client.Messaging.Fax and client.Actions.Fax.
    /// This class is not a runnable program — call these methods from your own application.
    /// Full parameter reference: docs/fax.md.
    /// </summary>
    public class FaxSamples
    {
        private readonly ITNZAuth apiUser;

        public FaxSamples()
        {
            apiUser = new TNZApiUser();
        }

        public FaxSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public FaxApiResult Send()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Fax.SendMessage(
                toNumber: "+6491111111",
                file: "MyDocument.pdf",                    // reads and base64-encodes the file — substitute your own path
                sendMode: Enums.SendModeType.Test          // TEST Mode - remove this to send live traffic
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

        public FaxApiResult SendUsingBuilder()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new FaxBuilder();

            var model = builder
                .AddDestination("+6491111111")
                .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Fax.SendMessage(model);

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

        public FaxApiResult SendUsingDestination()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new FaxBuilder();

            var model = builder
                .AddDestination(new Destination { Recipient = "+6491111111", Attention = "Alice" })
                .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Fax.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public FaxApiResult SendToMultipleRecipients()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new FaxBuilder();

            var model = builder
                .AddDestination("+6491111111")
                .AddDestination("+6492222222")
                .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Fax.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public FaxApiResult SendScheduled()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new FaxBuilder();

            var model = builder
                .AddDestination("+6491111111")
                .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
                .SetSendTime(DateTime.Now.AddDays(1))
                .SetTimezone("New Zealand")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Fax.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public FaxStatusApiResult Status(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Fax.Status(messageID);

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

        public FaxActionApiResult Reschedule(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Fax.Reschedule(messageID, DateTime.Now.AddHours(1));
        }

        public FaxActionApiResult Abort(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Fax.Abort(messageID);
        }

        public FaxActionApiResult Resubmit(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Fax.Resubmit(messageID, DateTime.Now);
        }
    }
}