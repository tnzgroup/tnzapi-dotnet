using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Voice;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using TNZAPI.NET.Core;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Samples.Messaging.Send
{
    public class SendVoice
    {
        private readonly ITNZAuth apiUser;

        public SendVoice(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public MessageApiResult Basic()
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

            return response;
        }

        public MessageApiResult Simple()
        {

            var client = new TNZApiClient(apiUser);

            var recipient = "+64211111111";

            var file = "D:\\File1.wav";

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var response = client.Messaging.Voice.SendMessage(
                groupIDs: new List<GroupID>()           // List of Addressbook Group IDs
                {
                    groupID
                },
                contactIDs: new List<ContactID>()       // List of Addressbook Contact IDs
                {
                    contactID
                },
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

            return response;
        }

        public MessageApiResult Builder()
        {
            //
            // Sample code using TTSApiBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var message = new VoiceBuilder()
                            .SetMessageToPeople("D:\\File1.wav")    // Message to People file name - WAV format, 16-bit, 8000hz
                            .AddRecipients(groupID)                 // Add Recipients by GroupID using TNZ Addressbook 
                            .AddRecipient(contactID)                // Add Recipient by ContactID using TNZ Addressbook 
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

            return response;
        }

        public MessageApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var reference = "Test Voice - Advanced version";

            var webhookCallbackURL = "https://example.com/webhook";
            var webhookCallbackFormat = Enums.WebhookCallbackType.XML;
            var errorEmailNotify = "notify@example.com";

            var callerId = "+6499999999";
            var billingAccount = "TEST BILLING ACCOUNT";
            var reportTo = "report@example.com";

            var recipient1 = "+64211111111";
            var recipient2 = "+64212222222";
            var recipient3 = "+64213333333";
            var recipient4 = "+64214444444";

            var messageToPeople = "D:\\File1.wav";                // WAV format, 16-bit, 8000hz
            var messageToAnswerphones = "D:\\File2.wav";          // WAV format, 16-bit, 8000hz
            var callRouteMessageToPeople = "D:\\File3.wav";       // WAV format, 16-bit, 8000hz
            var callRouteMessageToOperators = "D:\\File4.wav";    // WAV format, 16-bit, 8000hz
            var callRouteMessageOnWrongKey = "D:\\File5.wav";     // WAV format, 16-bit, 8000hz

            var numberOfOperators = 1;
            var retryAttempts = 2;
            var retryPeriod = 5;

            var keypad1Route = "+6491111111";
            var keypad2Route = "+6492222222";
            var keypad3Route = "+6493333333";
            var keypad4Route = "+6494444444";

            var keypad9PlaySection = KeypadPlaySection.Main;

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
            var keypad_list = new List<Keypad>();

            keypad_list.Add(new Keypad(3, keypad3Route));

            //
            // Add Keypad Method 4 - AddKeypad(new List()) using Keypad objects
            //
            var keypad4 = new Keypad();
            keypad4.Tone = 4;
            keypad4.RouteNumber = keypad4Route;

            keypad_list.Add(keypad4);

            //
            // Add Keypad Method 5 - Add Play (Wave Data)
            //
            var keypad5 = new Keypad();
            keypad5.Tone = 5;
            keypad5.PlayFile = "D:\\File1.wav";

            keypad_list.Add(keypad5);

            keypads.Add(keypad_list);

            //
            // Add Keypad Method 9 - Add Keypad 9 to play MessageToPeople (Main) section
            //

            keypads.Add(
                tone: 9,
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

            var response = client.Messaging.Voice.SendMessage(
                new VoiceModel()
                {
                    WebhookCallbackURL = webhookCallbackURL,            // Webhook Callback URL
                    WebhookCallbackFormat = webhookCallbackFormat,      // Webhook Callback Format (XML/JSON)

                    ErrorEmailNotify = errorEmailNotify,                // Error Email Notify (Receive email when it errored)

                    MessageID = new MessageID(""),                      // MessageID - Leave blank to auto-generate
                    Reference = reference,                              // Reference
                    CallerID = callerId,                                // Caller Id
                    SubAccount = billingAccount,                        // Billing Account
                    ReportTo = reportTo,                                // Report To

                    NumberOfOperators = numberOfOperators,              // Number of Operators
                    RetryAttempts = retryAttempts,                      // Retry Attempts
                    RetryPeriod = retryPeriod,                          // Retry Period

                    MessageData = voiceFiles,                           // List of voice files
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
