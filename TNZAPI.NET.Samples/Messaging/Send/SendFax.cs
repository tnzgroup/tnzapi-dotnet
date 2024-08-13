using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Fax;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Samples.Messaging.Send
{
    public class SendFax
    {
        private readonly ITNZAuth apiUser;

        public SendFax()
        {
            apiUser = new TNZApiUser();
        }

        public SendFax(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;
        }

        public MessageApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Messaging.Fax.SendMessage(
                destinations: new List<string>()
                {
                    "+6491111111",                      // Recipient 1
                    "+6491111112"                       // Recipient 2
                },
                file: "D:\\File1.pdf",                  // Attach File
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
            var client = new TNZApiClient(apiUser);         // Initializer

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var response = client.Messaging.Fax.SendMessage(
                groupIDs: new List<GroupID>()               // List of Addressbook Group IDs
                {
                    groupID
                },
                contactIDs: new List<ContactID>()           // List of Addressbook Contact IDs
                {
                    contactID
                },
                destination: "+6491111111",                 // Destination (Fax number);
                file: "D:\\File1.pdf",                      // File location
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
            // Sample code using FaxBuilder()
            //

            var client = new TNZApiClient(apiUser);

            var groupID = new GroupID("GGGGGGGG-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var contactID = new ContactID("CCCCCCCC-BBBB-BBBB-CCCC-DDDDDDDDDDDD");

            var message = new FaxBuilder()
                            .AddAttachment("D:\\File1.pdf")         // Attachment location
                            .AddRecipients(groupID)                 // Add Recipients by GroupID using TNZ Addressbook 
                            .AddRecipient(contactID)                // Add Recipient by ContactID using TNZ Addressbook 
                            .AddRecipient("+6491111111")            // Recipient
                            .SetSendMode(Enums.SendModeType.Test)   // TEST/Live mode
                            .Build();                               // Build Email() object

            var response = client.Messaging.Fax.SendMessage(message);

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

            var reference = "Test Fax - Advanced version";

            var webhookCallbackURL = "https://example.com/webhook";
            var webhookCallbackFormat = Enums.WebhookCallbackType.XML;
            var errorEmailNotify = "notify@example.com";

            var recipient1 = "+6491111111";
            var recipient2 = "+6492222222";
            var recipient3 = "+6493333333";
            var recipient4 = "+6494444444";

            var file1 = "D:\\file1.pdf";
            var file2 = "D:\\file2.pdf";
            var file3 = "D:\\file3.pdf";
            var file4 = "D:\\file4.pdf";

            var resolution = "High";
            var csid = "TEST FAX";

            var retryAttempts = 3;
            var retryPeriod = 1;

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
            // Add Attachment Method 3 - using simple file locations
            //

            attachments.Add(new Attachment(file3));

            //
            // Add Attachment Method 4 - using Attachment objects
            //

            attachments.Add(
                new Attachment(
                    FileHandlers.GetFileName(file4),
                    FileHandlers.GetFileContents(file4)
                )
            );

            #endregion Add Attachments

            var response = client.Messaging.Fax.SendMessage(
                new FaxModel()
                {
                    WebhookCallbackURL = webhookCallbackURL,            // Webhook Callback URL
                    WebhookCallbackFormat = webhookCallbackFormat,      // Webhook Callback Format (XML/JSON)

                    ErrorEmailNotify = errorEmailNotify,                // Error Email Notify (Receive email when it errored)

                    MessageID = new MessageID(""),                      // MessageID - Leave blank to auto-generate
                    Reference = reference,                              // Reference
                    SubAccount = "",                                    // SubAccount
                    Department = "",                                    // Department
                    ChargeCode = "",                                    // ChargeCode
                    Resolution = resolution,                            // Resolution - High/Low
                    CSID = csid,                                        // CSID

                    WatermarkFolder = "",                               // WaterMarkFolder
                    WatermarkFirstPage = "",                            // WaterMarkFirstPage
                    WatermarkAllPages = "",                             // WaterMarkAllPages

                    RetryAttempts = retryAttempts,                      // RetryAttempts - no of retries
                    RetryPeriod = retryPeriod,                          // RetryPeriod - no of minutes between retries

                    Recipients = recipients.ToList(),                   // Recipient list
                    Attachments = attachments.ToList(),                 // Attachment list

                    SendTime = DateTime.Now,                            // SendTime
                    Timezone = "New Zealand",                           // Timezone for SendTime

                    SendMode = Enums.SendModeType.Test
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
