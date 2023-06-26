using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.TTS;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Api.Messaging.Common.Enums;
using static TNZAPI.NET.Api.Messaging.TTS.Dto.TTSModel;

namespace TNZAPI.NET.Samples.Messaging.Send
{
    public class SendTTS
    {
        private readonly ITNZAuth apiUser;

        public SendTTS(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public void Basic()
        {
            // create TNZApiClient
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.TTS.SendMessage(
                messageToPeople: "Hello, this is a call from test. This is relevant information.", // Message to people
                destinations: new List<string>
                {
                    "+64211111111",                     // Recipient
                    "+64222222222",                     // Recipient
                },
                ttsVoiceType: TTSVoiceType.EnglishNZFemale1, // TTS Engine
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
            const string recipient = "+64211111111";                // Recipient

            const string messageToPeople = "Hello, this is a call from test. This is relevant information.";

            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.TTS.SendMessage(
                destination: recipient,                 // Recipient
                messageToPeople: messageToPeople,       // Message to people
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
            // Sample code using TTSBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var message = new TTSBuilder()
                            .SetMessageToPeople("Hello, this is a call from test. This is relevant information.")   // Message to People
                            .AddRecipient("+64211111111")           // Recipient
                            .SetSendMode(Enums.SendModeType.Test)   // TEST/Live mode
                            .Build();                               // Build TTS() object

            var response = client.Messaging.TTS.SendMessage(message);

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
            const string reference = "Test TTS - Advanced version";

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

            const string messageToPeople = "Hello, this is a call from Department01. This is relevant information. Press one to be connected to our call centre.";
            const string messageToAnswerphones = "Hello, sorry we missed you. This is a call from Department 01. Please contact us on 0800 123123.";
            const string callRouteMessageToPeople = "Connecting you now.";
            const string callRouteMessageToOperators = "Incoming Text To Speech call.";
            const string callRouteMessageOnWrongKey = "Sorry, you have pressed an invalid key. Please try again.";

            const int numberOfOperators = 1;
            const int retryAttempts = 3;
            const int retryPeriod = 1;

            const TTSVoiceType ttsVoice = TTSVoiceType.Female1;

            const string keypad1Route = "+6491111111";
            const string keypad2Route = "+6492222222";
            const string keypad3Play = "Hello, you have pressed 3.";
            const string keypad4Route = "+6493333333";
            const string keypad5Route = "+6494444444";
            const string keypad6Play = "Hello, you have pressed 5.";
            const string keypad7Route = "+6497777777";
            const string keypad7Play = "Hello, you have pressed 6.";

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
            // Add Keypad Method 3 - AddKeypad with Play data
            //
            keypads.Add(3, Keypad.KeypadType.Play, keypad3Play);

            //
            // Add Keypad Method 4 - AddKeypad(new List<IKeypad>())
            //
            keypads.Add(
                new List<Keypad>
                {
                    new Keypad(4, keypad4Route)
                });

            //
            // Add Keypad Method 5 - AddKeypad(new List<Keypad>()) using Keypad objects
            //
            Keypad keypad5 = new Keypad();
            keypad5.Tone = 5;
            keypad5.RouteNumber = keypad5Route;

            keypads.Add(keypad5);

            //
            // Add Keypad Method 6 - AddKeypad(new List()) with Play using Keypad objects
            //
            Keypad keypad6 = new Keypad();
            keypad6.Tone = 6;
            keypad6.Play = keypad6Play;

            keypads.Add(keypad6);

            //
            // Add Keypad Method 7 - AddKeypad(new List()) with RouteNumber and Play using Keypad objects
            //
            Keypad keypad7 = new Keypad();
            keypad7.Tone = 7;
            keypad7.Play = keypad7Play;
            keypad7.RouteNumber = keypad7Route;

            keypads.Add(keypad7);
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

            Recipient recipient = new Recipient(recipient2);
            recipient.CompanyName = "Test Company";         // Company Name
            recipient.Attention = "Test Recipient 2";       // Attention
            recipient.Custom1 = "Custom1";                  // Custom1
            recipient.Custom2 = "Custom2";                  // Custom2
            recipient.Custom3 = "Custom3";                  // Custom3
            recipient.Custom4 = "Custom4";                  // Custom4
            recipient.Custom5 = "Custom5";                  // Custom5

            recipients.Add(recipient);

            //
            // Add Recipient Method 3 - AddRecipients(new List<Recipient>()); using simple destination
            //

            var recipientList = new List<Recipient>();

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

            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.TTS.SendMessage(
                new TTSModel()
                {
                    WebhookCallbackURL = webhookCallbackURL,            // Webhook Callback URL
                    WebhookCallbackFormat = webhookCallbackFormat,      // Webhook Callback Format (XML/JSON)

                    ErrorEmailNotify = errorEmailNotify,                // Error Email Notify (Receive email when it errored)

                    MessageID = "",                                     // MessageID - Leave blank to auto-generate
                    Reference = reference,                              // Reference
                    SendTime = DateTime.Now,                            // SendTime
                    Timezone = "New Zealand",                           // Timezone
                    SubAccount = billingAccount,                        // Billing Account
                    Department = "",                                    // Department
                    ChargeCode = "",                                    // ChargeCode
                    CallerID = callerId,                                // Caller Id
                    ReportTo = reportTo,                                // Report To

                    NumberOfOperators = numberOfOperators,              // No. of operators - No. of calls at a time
                    RetryAttempts = retryAttempts,                      // Retry Attempts - number of retries
                    RetryPeriod = retryPeriod,                          // Retry Period - number of minutes between retries

                    TTSVoice = ttsVoice,                                // TTS Voice Engine
                    MessageToPeople = messageToPeople,                  // Message to People
                    MessageToAnswerPhones = messageToAnswerphones,      // Message to Answer Phones
                    CallRouteMessageToPeople = callRouteMessageToPeople,// Call Route Message to People (when call is routed)
                    CallRouteMessageToOperators = callRouteMessageToOperators,  // Call Route Message to AnswerPhones (when call is routed)
                    CallRouteMessageOnWrongKey = callRouteMessageOnWrongKey,    // Call Route Message on Wrong Key (when wrong key is entered)

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
