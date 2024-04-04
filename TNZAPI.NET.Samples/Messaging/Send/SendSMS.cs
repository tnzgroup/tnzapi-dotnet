using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.SMS;
using TNZAPI.NET.Api.Messaging.SMS.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Samples.Messaging.Send
{
    public class SendSMS
    {
        private readonly ITNZAuth apiUser;

        public SendSMS()
        {
            apiUser = new TNZApiUser();
        }

        public SendSMS(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public MessageApiResult Basic()
        {
            // Create TNZApiClient()

            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.SMS.SendMessage(
                destinations: new List<string>()
                {
                    "+64211111111",                         // Recipient
                    "+64222222222"                          // Recipient
                },
                messageText: "Test SMS",                    // SMS Message
                sendMode: Enums.SendModeType.Test           // TEST Mode - Remove this to send live traffic
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

            const string recipient = "+64211111111";

            const string message = "Hello, this is a test SMS from test.";

            var response = client.Messaging.SMS.SendMessage(
                destination: recipient,                     // Recipient
                messageText: message,                       // SMS Message
                sendMode: Enums.SendModeType.Test           // TEST Mode - Remove this to send live traffic
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
            // Sample code using SMSBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var message = new SMSBuilder()
                            .SetMessageText("Test SMS")             // SMS Message
                            .AddRecipient("+64211111111")           // Recipient
                            .SetSendMode(Enums.SendModeType.Test)   // TEST/Live mode
                            .Build();                               // Build SMS() object

            var response = client.Messaging.SMS.SendMessage(message);

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

            const string reference = "Test SMS - Advanced version";

            const string webhookCallbackURL = "https://example.com/webhook";
            const Enums.WebhookCallbackType webhookCallbackFormat = Enums.WebhookCallbackType.XML;
            const string errorEmailNotify = "notify@example.com";

            const string smsEmailReply = "reply@test.com";
            const string forceGSMChars = "True";

            const string recipient1 = "+64211111111";
            const string recipient2 = "+64212222222";
            const string recipient3 = "+64213333333";
            const string recipient4 = "+64214444444";

            const string file1 = "D:\\file1.pdf";
            const string file2 = "D:\\file2.pdf";
            const string file3 = "D:\\file3.pdf";
            const string file4 = "D:\\file4.pdf";

            const string messageText = "Test SMS Message [[File1]] | [[File2]] | [[File3]] | [[File4]]";

            #endregion Declarations

            #region Add Recipients

            var recipients = new RecipientList();

            //
            // Add Recipient Method 1 - Add(string recipient);
            //
            recipients.Add(recipient1);

            //
            // Add Recipient Method 2 - Add(new Recipient())
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
            // Add Recipient Method 3 - using simple destination
            //

            recipients.Add(new Recipient(recipient3));

            //
            // Add Recipient Method 4 - using Recipient objects
            //

            recipients.Add(new Recipient(
                recipient4,             // Recipient
                "Test Company",         // Company Name
                "Test Recipient 4",     // Attention
                "Custom1",              // Custom1
                "Custom2",              // Custom2
                "Custom3",              // Custom3
                "Custom4",              // Custom4
                "Custom5"               // Custom5
            ));

            #endregion Add Recipients

            #region Add Attachments
            /*
             * Please note:
             * 
             * Attachments are only supported with MessageLink - Please ask us to enable MessageLink 
             * 
             * Attachments will get ignored if you have not MessageLink functionality
             */

            var attachments = new AttachmentList();

            //
            // Add Attachment Method 1 - AddAttachment(file_location);
            //
            attachments.Add(file1);

            //
            // Add Attachment Method 2 - AddAttachment(new Attachment());
            //

            Attachment attachment = new Attachment();
            attachment.FileName = FileHandlers.GetFileName(file2);
            attachment.FileContent = FileHandlers.GetFileContents(file2);

            attachments.Add(attachment);

            //
            // Add Attachment Method 3 - AddAttachments(new List<IAttachment>()) using simple file locations
            //

            attachments.Add(new Attachment(file3));

            //
            // Add Attachment Method 4 - AddAttachments(new List<IAttachment>()) using Attachment objects
            //

            attachments.Add(new Attachment(
                FileHandlers.GetFileName(file4),
                FileHandlers.GetFileContents(file4)
                ));

            #endregion Add Attachments

            var response = client.Messaging.SMS.SendMessage(
                new SMSModel()
                {
                    WebhookCallbackURL = webhookCallbackURL,            // Webhook Callback URL
                    WebhookCallbackFormat = webhookCallbackFormat,      // Webhook Callback Format (XML/JSON)

                    ErrorEmailNotify = errorEmailNotify,                // Error Email Notify (Receive email when it errored)

                    MessageID = new MessageID(""),                      // MessageID - Leave blank to auto-generate
                    Reference = reference,                              // Reference
                    SubAccount = "",                                    // SubAccount
                    Department = "",                                    // Department
                    SMSEmailReply = smsEmailReply,                      // SMSEmailReply - For email (SMTP) reply receipt notifications
                    ForceGSMChars = forceGSMChars,                      // ForceGSMChars
                    MessageText = messageText,                          // SMS Message

                    Recipients = recipients.ToList(),                   // Recipient List
                    Attachments = attachments.ToList(),                 // Attachment List - Attachments only be supported with MessageLink facility

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
