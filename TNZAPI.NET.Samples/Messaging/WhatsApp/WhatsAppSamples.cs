using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.WhatsApp;
using TNZAPI.NET.Api.Messaging.WhatsApp.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.WhatsApp
{
    /// <summary>
    /// Reference code demonstrating client.Messaging.WhatsApp and client.Actions.WhatsApp.
    /// This class is not a runnable program — call these methods from your own application.
    /// Full parameter reference: docs/whatsapp.md.
    /// </summary>
    public class WhatsAppSamples
    {
        private readonly ITNZAuth apiUser;

        public WhatsAppSamples()
        {
            apiUser = new TNZApiUser();
        }

        public WhatsAppSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public WhatsAppApiResult Send()
        {
            // WhatsApp requires BOTH Message and TemplateID (unlike SMS/RCS's either/or) —
            // find your Template ID in the TNZ Dashboard.
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.WhatsApp.SendMessage(
                messageText: "Hi [[FirstName]], your order has shipped!",
                templateId: "123e4567-e89b-12d3-a456-426614174000",
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

        public WhatsAppApiResult SendUsingBuilder()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new WhatsAppBuilder();

            var model = builder
                .SetMessageText("Hi [[FirstName]], your order has shipped!")
                .SetTemplateID("123e4567-e89b-12d3-a456-426614174000")
                .AddDestination("+64211111111")
                .AddFallbackMode(Enums.WhatsAppFallbackMode.SMS)
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.WhatsApp.SendMessage(model);
        }

        public WhatsAppApiResult SendUsingDestination()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new WhatsAppBuilder();

            var model = builder
                .SetMessageText("Hi [[FirstName]], your order has shipped!")
                .SetTemplateID("123e4567-e89b-12d3-a456-426614174000")
                .AddDestination(new Destination { Recipient = "+64211111111", FirstName = "Alice" })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.WhatsApp.SendMessage(model);
        }

        public WhatsAppApiResult SendToMultipleRecipients()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new WhatsAppBuilder();

            var model = builder
                .SetMessageText("Hi [[FirstName]], your order has shipped!")
                .SetTemplateID("123e4567-e89b-12d3-a456-426614174000")
                .AddDestination("+64211111111")
                .AddDestination("+64222222222")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.WhatsApp.SendMessage(model);
        }

        public WhatsAppApiResult SendWithAttachment()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new WhatsAppBuilder();

            var model = builder
                .SetMessageText("Here's your invoice.")
                .SetTemplateID("123e4567-e89b-12d3-a456-426614174000")
                .AddDestination("+64211111111")
                .AddAttachment("Invoice.pdf", "[base64-encoded-file-data]")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            return client.Messaging.WhatsApp.SendMessage(model);
        }

        public WhatsAppStatusApiResult Status(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.WhatsApp.Status(messageID);

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

        public WhatsAppReceivedApiResult Received()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.WhatsApp.Received(timePeriod: 1440);

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var message in response.Messages)
                {
                    Console.WriteLine($" => From: '{message.From}', MessageText: '{message.MessageText}'");
                }
            }

            return response;
        }

        public WhatsAppReceivedApiResult ReceivedByDateRange()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.WhatsApp.Received(dateFrom: DateTime.Now.AddDays(-1), recordsPerPage: 100, page: 1);

            if (response.Result == Enums.ResultCode.Success)
            {
                foreach (var message in response.Messages)
                {
                    Console.WriteLine($" => From: '{message.From}', MessageText: '{message.MessageText}'");
                }
            }

            return response;
        }

        public WhatsAppActionApiResult Reschedule(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.WhatsApp.Reschedule(messageID, DateTime.Now.AddHours(1));
        }

        public WhatsAppActionApiResult Abort(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.WhatsApp.Abort(messageID);
        }
    }
}