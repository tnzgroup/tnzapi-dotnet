using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Email
{
    /// <summary>
    /// Reference code demonstrating client.Messaging.Email and client.Actions.Email.
    /// This class is not a runnable program — call these methods from your own application.
    /// Full parameter reference: docs/email.md.
    /// </summary>
    public class EmailSamples
    {
        private readonly ITNZAuth apiUser;

        public EmailSamples()
        {
            apiUser = new TNZApiUser();
        }

        public EmailSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public EmailApiResult Send()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Email.SendMessage(
                fromEmail: "from@test.com",             // Optional - leave blank to use your API username as sender
                emailSubject: "Test Email",
                messagePlain: "Test Email Body",
                emailAddress: "email.one@test.com",
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

        public EmailApiResult SendUsingBuilder()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new EmailBuilder();

            var model = builder
                .SetEmailSubject("Test Email")
                .SetMessagePlain("Test Email Body")
                .AddDestination("email.one@test.com")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Email.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public EmailApiResult SendUsingDestination()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new EmailBuilder();

            var model = builder
                .SetEmailSubject("Test Email")
                .SetMessagePlain("Test Email Body")
                .AddDestination(new Destination { Recipient = "email.one@test.com", FirstName = "Alice" })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Email.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public EmailApiResult SendHtmlEmail()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new EmailBuilder();

            var model = builder
                .SetEmailSubject("Test Email")
                .SetMessageHTML("<p>Test <strong>Email</strong> Body</p>")
                .AddDestination("email.one@test.com")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Email.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public EmailApiResult SendWithAttachment()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new EmailBuilder();

            var model = builder
                .SetEmailSubject("Test Email")
                .SetMessagePlain("See attached.")
                .AddDestination("email.one@test.com")
                .AddAttachment("My Document.pdf", "[base64-encoded-file-data]")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Email.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public EmailStatusApiResult Status(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Email.Status(messageID);

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

        public EmailActionApiResult Reschedule(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Actions.Email.Reschedule(messageID, DateTime.Now.AddHours(1));

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Action: '{response.Action}', Status: '{response.Status}'");
            }

            return response;
        }

        public EmailActionApiResult Abort(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Email.Abort(messageID);
        }

        public EmailActionApiResult Resubmit(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Email.Resubmit(messageID, DateTime.Now);
        }
    }
}