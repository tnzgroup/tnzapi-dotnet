using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
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

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var message = "Hello, this is a test SMS from test.";

            var response = client.Messaging.SMS.SendMessage(
                groupIDs: new List<GroupID>()               // List of Addressbook Group IDs
                {
                    groupID
                },
                contactIDs: new List<ContactID>()           // List of Addressbook Contact IDs
                {
                    contactID
                },
                destinations: new List<string>() {
                    "+64211111111",                         // Recipient 1
                    "+64222222222"                          // Recipient 2
                },
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

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var message = new SMSBuilder()
                            .SetMessageText("Test SMS")             // SMS Message
                            .AddRecipients(groupID)                 // Add Recipients by GroupID using TNZ Addressbook 
                            .AddRecipient(contactID)                // Add Recipient by ContactID using TNZ Addressbook
                            .AddRecipient("+64211111111")           // Recipient
                            .AddRecipients(new List<Recipient>())
                            .SetSendMode(Enums.SendModeType.Test)   // TEST/Live mode
                            .Build();                               // Build SMS() object

            var response = client.Messaging.SMS.SendMessage(message);

            DebugUtil.Dump(response);

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

            var reference = "Test SMS - Advanced version";

            var webhookCallbackURL = "https://example.com/webhook";
            var webhookCallbackFormat = Enums.WebhookCallbackType.XML;
            var reportTo = "notify@example.com";

            var smsEmailReply = "reply@test.com";
            var forceGSMChars = "True";

            var recipient1 = "+64211111111";
            var recipient2 = "+64212222222";
            var recipient3 = "+64213333333";
            var recipient4 = "+64214444444";

            var file1 = "D:\\file1.pdf";
            var file2 = "D:\\file2.pdf";
            var file3 = "D:\\file3.pdf";
            var file4 = "D:\\file4.pdf";

            var messageText = "Test SMS Message [[File1]] | [[File2]] | [[File3]] | [[File4]]";

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

            var attachment = new Attachment();
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

            attachments.Add(
                new Attachment(
                    FileHandlers.GetFileName(file4),
                    FileHandlers.GetFileContents(file4)
                )
            );

            #endregion Add Attachments

            var response = client.Messaging.SMS.SendMessage(
                new SMSModel()
                {
                    WebhookCallbackURL = webhookCallbackURL,            // Webhook Callback URL
                    WebhookCallbackFormat = webhookCallbackFormat,      // Webhook Callback Format (XML/JSON)

                    MessageID = new MessageID("ABCD12345"),             // MessageID - Leave blank to auto-generate
                    Reference = reference,                              // Reference
                    SubAccount = "",                                    // SubAccount
                    Department = "",                                    // Department
                    SMSEmailReply = smsEmailReply,                      // SMSEmailReply - For email (SMTP) reply receipt notifications
										ReportTo = reportTo,                                // Notification email address (Receive email for notifications)
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
