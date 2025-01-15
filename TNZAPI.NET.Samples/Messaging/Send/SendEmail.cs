using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Email;
using TNZAPI.NET.Api.Messaging.Email.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Samples.Messaging.Send
{
    public class SendEmail
    {
        private readonly ITNZAuth apiUser;

        public SendEmail()
        {
            apiUser = new TNZApiUser();
        }

        public SendEmail(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public MessageApiResult Basic()
        {
            //
            // Use AuthToken (can find it from our web portal) if you prefer
            //

            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Email.SendMessage(
              fromEmail: "from@test.com",             // Optional : Sets From Email Address - leave blank to use your api username as email sender
              emailSubject: "Test Email",             // Email Subject
              messagePlain: "Test Email Body",        // Email Body
              destination: "email.one@test.com",      // Recipient 1
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

            var response = client.Messaging.Email.SendMessage(
                fromEmail: "from@test.com",             // Optional : Sets From Email Address - leave blank to use your api username as email sender
                emailSubject: "Test Email",             // Email Subject
                messagePlain: "Test Email Body",        // Email Body
                groupIDs: new List<GroupID>()           // List of Addressbook Group IDs
                {
                    new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
                },
                contactIDs: new List<ContactID>()       // List of Addressbook Contact IDs
                {
                    new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD")
                },
                destinations: new List<string>() {
                    "email.one@test.com",               // Recipient 1
                    "email.two@test.com"                // Recipient 2
                },
                files: new List<string>()
                {
                    "d:\\File1.pdf",                    // Attachment 1
                    "d:\\File2.pdf"                     // Attachment 2
                },
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
            // Sample code using EmailBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var message = new EmailBuilder()
                    .SetEmailFrom("from@test.com")          // Optional : Sets From Email Address - leave blank to use your api username as email sender
                    .SetEmailSubject("Test Email")          // Email Subject
                    .SetMessagePlain("Test Email Body")     // Email Body (Plain Text)
                    .AddRecipients(groupID)                 // Add Recipients by GroupID using TNZ Addressbook 
                    .AddRecipient(contactID)                // Add Recipient by ContactID using TNZ Addressbook 
                    .AddRecipient("to@test.com")            // Recipient (Email Address)
                    .AddAttachment("d:\\File1.pdf")         // Attachment
                    .SetSendMode(Enums.SendModeType.Test)   // TEST/Live mode
                    .Build();                               // Build Email() object

            var response = client.Messaging.Email.SendMessage(message);

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

            var messageID = new MessageID("ABCD12345");

            var reference = "Test Email - Advanced version";

            var webhookCallbackURL = "https://example.com/webhook";
            var webhookCallbackFormat = Enums.WebhookCallbackType.XML;
            var reportTo = "notify@example.com";

            var recipient1 = "emailTo@test.com";
            var recipient2 = "emailTo@test.com";
            var recipient3 = "emailTo@test.com";
            var recipient4 = "emailTo@test.com";

            var file1 = "d:\\File1.pdf";
            var file2 = "d:\\File2.pdf";
            var file3 = "d:\\File3.pdf";
            var file4 = "d:\\File4.pdf";

            var smtpFrom = "from@test.com";
            var fromName = "Email From";
            var fromEmail = "email@test.com";
            var replyTo = "email@test.com";

            var subject = "Test Email 123";
            var messagePlain = "Test Email Body";
            var messageHtml = "This is Test message body. Thank you so much!";

            var recipients = new RecipientList();

            #endregion Declarations

            #region Add Recipients

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

            #region Add Attachments

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
            // Add Attachment Method 3 - AddAttachments(new List<Attachment>()) using simple file locations
            //

            attachments.Add(new Attachment(file3));

            //
            // Add Attachment Method 4 - AddAttachments(new List<Attachment>()) using Attachment objects
            //

            attachments.Add(
                new Attachment(
                    FileHandlers.GetFileName(file4),
                    FileHandlers.GetFileContents(file4)
                )
            );

            #endregion Add Attachments

            var response = client.Messaging.Email.SendMessage(
                new EmailModel()
                {
                    MessageID = messageID,                              // Message/Tracking ID, leave blank to auto-generate

                    WebhookCallbackURL = webhookCallbackURL,            // Webhook Callback URL
                    WebhookCallbackFormat = webhookCallbackFormat,      // Webhook Callback Format (XML/JSON)

                    ReportTo = reportTo,                                // Notification email address (Receive email for notifications)

										EmailSubject = subject,                             // Subject
                    MessagePlain = messagePlain,                        // MessagePlain
                    MessageHTML = messageHtml,                          // MessageHTML
                    Reference = reference,                              // Reference

                    // Set SMTP Headers
                    SMTPFrom = smtpFrom,                                // SMTP From
                    From = fromName,                                    // From Name
                    FromEmail = fromEmail,                              // From Email
                    ReplyTo = replyTo,                                  // Reply-To

                    Recipients = recipients.ToList(),                   // Recipient List
                    Attachments = attachments.ToList(),                 // Attachment List

                    SendTime = DateTime.Now,                            // SendTime
                    Timezone = "New Zealand",                           // Timezone for SendTime

                    SendMode = Enums.SendModeType.Test                  // TEST Mode - Remove this to send live traffic
                }
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
    }
}
