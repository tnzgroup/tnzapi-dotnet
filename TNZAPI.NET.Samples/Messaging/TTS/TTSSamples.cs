using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.TTS
{
    /// <summary>
    /// Reference code demonstrating client.Messaging.TTS and client.Actions.TTS.
    /// This class is not a runnable program — call these methods from your own application.
    /// Full parameter reference: docs/tts.md.
    /// </summary>
    public class TTSSamples
    {
        private readonly ITNZAuth apiUser;

        public TTSSamples()
        {
            apiUser = new TNZApiUser();
        }

        public TTSSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public TTSApiResult Send()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.TTS.SendMessage(
                messageToPeople: "Hello, this is a call from test. This is relevant information.",
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

        public TTSApiResult SendWithKeypadMenu()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new TTSBuilder();

            var model = builder
                .SetMessageToPeople("Press 1 for sales, press 2 for support.")
                .AddDestination("+64211111111")
                .AddKeypad(new KeypadModel
                {
                    Tone = 1,
                    Play = "Connecting you to sales now.",
                    PlaySection = Enums.KeypadPlaySection.Main,
                    RouteNumber = "+64211112222"
                })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.TTS.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public TTSApiResult SendUsingDestination()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new TTSBuilder();

            var model = builder
                .SetMessageToPeople("Hello, this is a call from test.")
                .AddDestination(new Destination { Recipient = "+64211111111", FirstName = "Alice" })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.TTS.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public TTSApiResult SendToMultipleRecipients()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new TTSBuilder();

            var model = builder
                .SetMessageToPeople("Hello, this is a call from test.")
                .AddDestination("+64211111111")
                .AddDestination("+64222222222")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.TTS.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public TTSApiResult SendScheduled()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new TTSBuilder();

            var model = builder
                .SetMessageToPeople("Your reminder call.")
                .AddDestination("+64211111111")
                .SetSendTime(DateTime.Now.AddDays(1))
                .SetTimezone("New Zealand")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.TTS.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public TTSStatusApiResult Status(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.TTS.Status(messageID);

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

        public TTSActionApiResult Reschedule(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.TTS.Reschedule(messageID, DateTime.Now.AddHours(1));
        }

        public TTSActionApiResult Abort(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.TTS.Abort(messageID);
        }

        public TTSActionApiResult Resubmit(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.TTS.Resubmit(messageID, DateTime.Now);
        }

        public TTSActionApiResult Pacing(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            // Adjust the number of simultaneous operators on an in-progress job
            return client.Actions.TTS.Pacing(messageID, numberOfOperators: 1);
        }
    }
}