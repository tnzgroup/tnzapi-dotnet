using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Voice;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Api.Messaging.Common.Enums;

namespace TNZAPI.NET.Samples.Messaging.Send
{
    public class SendVoice
    {
        private readonly ITNZAuth apiUser;

        public SendVoice(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public void Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Voice.SendMessage(
                destinations: new List<string>()
                {
                    "+64211111111",                     // Recipient
                    "+64222222222"                      // Recipient
                },
                messageToPeople: "D:\\File1.wav",       // WAV format, 16-bit, 8000hz
                sendMode: Enums.SendModeType.Test       // TEST Mode - Remove this to send live traffic
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Success - " + response.MessageID);
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

        public void Simple()
        {
            const string recipient = "+64211111111";

            const string file = "D:\\File1.wav";

            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Voice.SendMessage(
                destination: recipient,                 // Recipient
                messageToPeople: file,                  // WAV format, 16-bit, 8000hz
                sendMode: Enums.SendModeType.Test       // TEST Mode - Remove this to send live traffic
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Success - " + response.MessageID);
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

        public void Builder()
        {
            //
            // Sample code using TTSApiBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var message = new VoiceBuilder()
                            .SetMessageToPeople("D:\\File1.wav")    // Message to People file name - WAV format, 16-bit, 8000hz
                            .AddRecipient("+64211111111")           // Recipient
                            .SetSendMode(Enums.SendModeType.Test)   // TEST/Live mode
                            .Build();                               // Build TTS() object

            var response = client.Messaging.Voice.SendMessage(message);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Success - " + response.MessageID);
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

        public void Advanced()
        {
            const string reference = "Test Voice - Advanced version";

            const string webhookCallbackURL = "https://example.com/webhook";
            const Enums.WebhookCallbackType webhookCallbackFormat = Enums.WebhookCallbackType.XML;
            const string errorEmailNotify = "notify@example.com";

            const string callerId = "+6499999999";
            const string billingAccount = "TEST BILLING ACCOUNT";
            const string reportTo = "report@example.com";

            const string recipient1 = "+64211111111";
            const string recipient2 = "+64212222222";
            const string recipient3 = "+64213333333";
            const string recipient4 = "+64214444444";

            const string messageToPeople = "D:\\File1.wav";                // WAV format, 16-bit, 8000hz
            const string messageToAnswerphones = "D:\\File2.wav";          // WAV format, 16-bit, 8000hz
            const string callRouteMessageToPeople = "D:\\File3.wav";       // WAV format, 16-bit, 8000hz
            const string callRouteMessageToOperators = "D:\\File4.wav";    // WAV format, 16-bit, 8000hz
            const string callRouteMessageOnWrongKey = "D:\\File5.wav";     // WAV format, 16-bit, 8000hz

            const int numberOfOperators = 1;
            const int retryAttempts = 2;
            const int retryPeriod = 5;

            const string keypad1Route = "+6491111111";
            const string keypad2Route = "+6492222222";
            const string keypad3Route = "+6493333333";
            const string keypad4Route = "+6494444444";

            #region Add Keypads

            var keypads = new KeypadList();

            //
            // Add Keypad Method 1 - VoiceMessage.AddKeypad(int key, string keypad1_route);
            //
            keypads.Add(1, keypad1Route);

            //
            // Add Keypad Method 2 - AddKeypad(new Keypad());
            //
            keypads.Add(new Keypad(2, keypad2Route));

            //
            // Add Keypad Method 3 - AddKeypad(new List<IKeypad>())
            //
            List<Keypad> keypad_list = new List<Keypad>();

            keypad_list.Add(new Keypad(3, keypad3Route));

            //
            // Add Keypad Method 4 - AddKeypad(new List()) using Keypad objects
            //
            Keypad keypad4 = new Keypad();
            keypad4.Tone = 4;
            keypad4.RouteNumber = keypad4Route;

            keypad_list.Add(keypad4);

            //
            // Add Keypad Method 5 - Add Play (Wave Data)
            //
            Keypad keypad5 = new Keypad();
            keypad5.Tone = 5;
            keypad5.PlayFile = "D:\\File1.wav";

            keypad_list.Add(keypad5);


            keypads.Add(keypad_list);
            #endregion Add Keypads

            #region Add Recipients

            var recipients = new RecipientList();

            //
            // Add Recipient Method 1 - AddRecipient(string recipient);
            //

            recipients.Add(recipient1);

            //
            // Add Recipient Method 2 - AddRecipient(new Recipient())
            //

            var recipient = new Recipient(recipient2);
            recipient.CompanyName = "Test Company";         // Company Name
            recipient.Attention = "Test Recipient 2";       // Attention
            recipient.Custom1 = "Custom1";                  // Custom1
            recipient.Custom2 = "Custom2";                  // Custom2
            recipient.Custom3 = "Custom3";                  // Custom3
            recipient.Custom4 = "Custom4";                  // Custom4
            recipient.Custom5 = "Custom5";                  // Custom5

            //VoiceMessage.AddRecipient(recipient);

            //
            // Add Recipient Method 3 - AddRecipients(new List<IRecipient>()); using simple destination
            //

            List<Recipient> recipientList = new List<Recipient>();

            recipientList.Add(new Recipient(recipient3));

            //
            // Add Recipient Method 4 - AddRecipients(new List<Recipient>()) using Recipient objects
            //

            recipientList.Add(new Recipient(
                recipient4,             // Recipient
                "Test Company",         // Company Name
                "Test Recipient 4",     // Attention
                "Custom1",              // Custom1
                "Custom2",              // Custom2
                "Custom3",              // Custom3
                "Custom4",              // Custom4
                "Custom5"               // Custom5
            ));

            recipients.Add(recipientList);

            #endregion Add Recipients

            #region Attach Voicefiles

            var voiceFiles = new Dictionary<MessageDataType, string>()
            {
                { MessageDataType.MessageToPeople, messageToPeople },
                { MessageDataType.MessageToAnswerPhones, messageToAnswerphones },
                { MessageDataType.CallRouteMessageToPeople, callRouteMessageToPeople },
                { MessageDataType.CallRouteMessageToOperators, callRouteMessageToOperators },
                { MessageDataType.CallRouteMessageOnWrongKey, callRouteMessageOnWrongKey }
            };

            #endregion

            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Voice.SendMessage(
                new VoiceModel()
                {
                    WebhookCallbackURL = webhookCallbackURL,            // Webhook Callback URL
                    WebhookCallbackFormat = webhookCallbackFormat,      // Webhook Callback Format (XML/JSON)

                    ErrorEmailNotify = errorEmailNotify,                // Error Email Notify (Receive email when it errored)

                    MessageID = "",                                     // MessageID - Leave blank to auto-generate
                    Reference = reference,                              // Reference
                    CallerID = callerId,                                // Caller Id
                    SubAccount = billingAccount,                        // Billing Account
                    ReportTo = reportTo,                                // Report To

                    NumberOfOperators = numberOfOperators,              // Number of Operators
                    RetryAttempts = retryAttempts,                      // Retry Attempts
                    RetryPeriod = retryPeriod,                          // Retry Period

                    MessageData = voiceFiles,                           // List of voice files
                    Keypads = keypads.ToList(),                         // Keypads (1..9)
                    Recipients = recipients.ToList(),                   // Recipients

                    SendMode = Enums.SendModeType.Test                  // TEST Mode - Remove this to send live traffic
                });

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine("Success - " + response.MessageID);
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
