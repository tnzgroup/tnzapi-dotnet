using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Samples.Messaging.Send
{
    public class SendTTS
    {
        private readonly ITNZAuth apiUser;

        public SendTTS(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public MessageApiResult Basic()
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

            return response;
        }

        public MessageApiResult Simple()
        {
            var client = new TNZApiClient(apiUser);

            var recipient = "+64211111111";                // Recipient

            var messageToPeople = "Hello, this is a call from test. This is relevant information.";

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var response = client.Messaging.TTS.SendMessage(
                groupIDs: new List<GroupID>()           // List of Addressbook Group IDs
                {
                    groupID
                },
                contactIDs: new List<ContactID>()       // List of Addressbook Contact IDs
                {
                    contactID
                },
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

            return response;
        }

        public MessageApiResult Builder()
        {
            //
            // Sample code using TTSBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var message = new TTSBuilder()
                            .SetMessageToPeople("Hello, this is a call from test. This is relevant information.")   // Message to People
                            .AddRecipients(groupID)                 // Add Recipients by GroupID using TNZ Addressbook 
                            .AddRecipient(contactID)                // Add Recipient by ContactID using TNZ Addressbook 
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

            return response;
        }

        public MessageApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            #region Declarations

            var reference = "Test TTS - Advanced version";

            var webhookCallbackURL = "https://example.com/webhook";
            var webhookCallbackFormat = Enums.WebhookCallbackType.XML;

            var callerId = "+6499999999";
            var billingAccount = "TEST BILLING ACCOUNT";
            var reportTo = "report@example.com";

            var recipient1 = "+64211111111";
            var recipient2 = "+64212222222";
            var recipient3 = "+64213333333";
            var recipient4 = "+64214444444";

            var messageToPeople = "Hello, this is a call from Department01. This is relevant information. Press one to be connected to our call centre.";
            var messageToAnswerphones = "Hello, sorry we missed you. This is a call from Department 01. Please contact us on 0800 123123.";
            var callRouteMessageToPeople = "Connecting you now.";
            var callRouteMessageToOperators = "Incoming Text To Speech call.";
            var callRouteMessageOnWrongKey = "Sorry, you have pressed an invalid key. Please try again.";

            var numberOfOperators = 1;
            var retryAttempts = 3;
            var retryPeriod = 1;

            var ttsVoice = TTSVoiceType.Female1;

            var keypad1Route = "+6491111111";
            var keypad2Route = "+6492222222";
            var keypad3Play = "Hello, you have pressed 3.";
            var keypad4Route = "+6493333333";
            var keypad5Route = "+6494444444";
            var keypad6Play = "Hello, you have pressed 5.";
            var keypad7Route = "+6497777777";
            var keypad7Play = "Hello, you have pressed 6.";
            var keypad9PlaySection = KeypadPlaySection.Main;

            #endregion

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

            //
            // Add Keypad Method 9 - Add Keypad 9 to play MessageToPeople (Main) section
            //

            keypads.Add(
                tone:9, 
                playSection: keypad9PlaySection
            );

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

            recipients.Add(recipient);

            //
            // Add Recipient Method 3 - AddRecipients(new List<string>()); using simple destination
            //

            recipients.Add(new List<string>() { recipient3 });

            //
            // Add Recipient Method 4 - AddRecipients(new List<Recipient>()) using Recipient objects
            //

            recipients.Add(
                new List<Recipient>()
                {
                    new Recipient(
                        recipient4,             // Recipient
                        "Test Company",         // Company Name
                        "Test Recipient 3",     // Attention
                        "Custom1",              // Custom1
                        "Custom2",              // Custom2
                        "Custom3",              // Custom3
                        "Custom4",              // Custom4
                        "Custom5"               // Custom5
                    )
                }
            );

            #endregion Add Recipients

            #region Add Recipients using TNZ Addressbook

            //
            // Add Recipient Method 5 - Add Recipients using GroupID
            //

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            recipients.Add(groupID);

            //
            // Add Recipient Method 6 - Add Recipients using list of GroupIDs
            //

            var groupIDs = new List<GroupID>()
            {
                new GroupID("HHHHHHHH-BBBB-BBBB-CCCC-DDDDDDDDDDDD"),
                new GroupID("IIIIIIII-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
            };

            recipients.Add(groupIDs);

            //
            // Add Recipient Method 7 - Add Recipient using ContactID 
            //

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            recipients.Add(contactID);

            //
            // Add Recipient Method 8 - Add Recipients using list of ContactIDs
            //

            var contactIDs = new List<ContactID>()
            {
                new ContactID("DDDDDDDD-BBBB-BBBB-CCCC-DDDDDDDDDDDD"),
                new ContactID("EEEEEEEE-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
            };

            recipients.Add(contactIDs);

            #endregion

            var response = client.Messaging.TTS.SendMessage(
                new TTSModel()
                {
                    WebhookCallbackURL = webhookCallbackURL,            // Webhook Callback URL
                    WebhookCallbackFormat = webhookCallbackFormat,      // Webhook Callback Format (XML/JSON)

                    MessageID = new MessageID(""),                      // MessageID - Leave blank to auto-generate
                    Reference = reference,                              // Reference
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
                    KeypadOptionRequired = true,                        // Requires the callee presses a keypad option
                    Recipients = recipients.ToList(),                   // Recipients

                    SendTime = DateTime.Now,                            // SendTime
                    Timezone = "New Zealand",                           // Timezone for SendTime

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

            return response;
        }
    }
}
