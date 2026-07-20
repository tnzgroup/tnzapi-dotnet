using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Api.Messaging.Voice;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Messaging.Voice
{
    /// <summary>
    /// Reference code demonstrating client.Messaging.Voice and client.Actions.Voice.
    /// This class is not a runnable program — call these methods from your own application.
    /// Full parameter reference: docs/voice.md.
    /// </summary>
    public class VoiceSamples
    {
        private readonly ITNZAuth apiUser;

        public VoiceSamples()
        {
            apiUser = new TNZApiUser();
        }

        public VoiceSamples(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public VoiceApiResult Send()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Voice.SendMessage(
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

        public VoiceApiResult SendWithKeypadMenu()
        {
            // Voice reuses TTS's KeypadModel/Enums.AnswerPhoneMode/Enums.KeypadPlaySection — same shape, different channel.
            var client = new TNZApiClient(apiUser);

            using var builder = new VoiceBuilder();

            var model = builder
                .AddDestination("+64211111111")
                .AddKeypad(new KeypadModel
                {
                    Tone = 1,
                    RouteNumber = "+64211112222",
                    PlaySection = Enums.KeypadPlaySection.Main
                })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Voice.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public VoiceApiResult SendUsingDestination()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new VoiceBuilder();

            var model = builder
                .AddDestination(new Destination { Recipient = "+64211111111", FirstName = "Alice" })
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Voice.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public VoiceApiResult SendToMultipleRecipients()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new VoiceBuilder();

            var model = builder
                .AddDestination("+64211111111")
                .AddDestination("+64222222222")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Voice.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public VoiceApiResult SendScheduled()
        {
            var client = new TNZApiClient(apiUser);

            using var builder = new VoiceBuilder();

            var model = builder
                .AddDestination("+64211111111")
                .SetSendTime(DateTime.Now.AddDays(1))
                .SetTimezone("New Zealand")
                .SetSendMode(Enums.SendModeType.Test)
                .Build();

            var response = client.Messaging.Voice.SendMessage(model);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Success - MessageID: {response.MessageID}");
            }

            return response;
        }

        public VoiceStatusApiResult Status(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Voice.Status(messageID);

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

        public VoiceActionApiResult Reschedule(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Voice.Reschedule(messageID, DateTime.Now.AddHours(1));
        }

        public VoiceActionApiResult Abort(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Voice.Abort(messageID);
        }

        public VoiceActionApiResult Resubmit(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Voice.Resubmit(messageID, DateTime.Now);
        }

        public VoiceActionApiResult Pacing(MessageID messageID)
        {
            var client = new TNZApiClient(apiUser);

            return client.Actions.Voice.Pacing(messageID, numberOfOperators: 1);
        }
    }
}