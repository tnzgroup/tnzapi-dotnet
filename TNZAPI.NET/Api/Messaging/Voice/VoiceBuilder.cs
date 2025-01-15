using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.Voice.Dto;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.Voice
{
		public class VoiceBuilder : IDisposable
    {
        private VoiceModel Entity { get; set; }

        private RecipientList Recipients { get; set; }

        private KeypadList Keypads { get; set; }

        /// <summary>
        /// Builder builds Voice message to send through TNZAPI
        /// </summary>
        public VoiceBuilder()
        {
            Entity = new VoiceModel();
            Recipients = new RecipientList();
            Keypads = new KeypadList();
        }


        /// <summary>
        /// Dispose Builder
        /// </summary>
        public void Dispose()
        {
            Entity = null;
            Recipients = null;
            Keypads = null;
        }

				#region General

				/// <summary>
				/// Sets ErrorEmailNotify, email address to get error notifications
				/// </summary>
				/// <param name="emailAddress">Your email address</param>
				/// <returns>VoiceBuilder</returns>
				[Obsolete("Use SetReportTo() instead of SetErrorEmailNotify()")]
				public VoiceBuilder SetErrorEmailNotify(string emailAddress)
        {
            Entity.ErrorEmailNotify = emailAddress;

            return this;
        }

				/// <summary>
				/// Sets report to email address
				/// </summary>
				/// <param name="emailAddress">Email address</param>
				/// <returns>VoiceBuilder</returns>
				public VoiceBuilder SetReportTo(string emailAddress)
				{
						Entity.ReportTo = emailAddress;

						return this;
				}

				/// <summary>
				/// Sets Webhook Callback URL to receive webhooks
				/// </summary>
				/// <param name="url">Your URL to receive webhooks</param>
				/// <returns>VoiceBuilder</returns>
				public VoiceBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback Format - JSON/XML
        /// </summary>
        /// <param name="type">WebhookCallbackType</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetWebhookCallbackFormat(WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        /// <summary>
        /// Sets Send mode - Live/Text
        /// </summary>
        /// <param name="mode">SendModeType</param>
        /// <returns>TTSBuilder</returns>
        public VoiceBuilder SetSendMode(SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        /// <summary>
        /// Sets MessageID of the message
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

        /// <summary>
        /// Sets MessageID of the message
        /// </summary>
        /// <param name="messageID">MessageID</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetMessageID(MessageID messageID)
        {
            Entity.MessageID = messageID;

            return this;
        }

        /// <summary>
        /// Sets Reference of the message
        /// </summary>
        /// <param name="reference"></param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        /// <summary>
        /// Sets Send Time of the message
        /// </summary>
        /// <param name="TTSBuilder">DateTime</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        /// <summary>
        /// Sets Timezone with SendTime
        /// </summary>
        /// <param name="timezone">string</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        /// <summary>
        /// Sets SubAccount value of the message
        /// </summary>
        /// <param name="subaccount">SubAccount value</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        /// <summary>
        /// Sets Department value of the message
        /// </summary>
        /// <param name="department">Department value</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        /// <summary>
        /// Sets ChargeCode value of the message
        /// </summary>
        /// <param name="chargeCode">Charge Code Value</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }

				/// <summary>
				/// Sets ServiceName value of the message
				/// </summary>
				/// <param name="serviceName">Service Name Value</param>
				/// <returns>VoiceBuilder</returns>
				public VoiceBuilder SetServiceName(string serviceName)
				{
						Entity.ServiceName = serviceName;

						return this;
				}

				#endregion

				#region Voice Specific

				/// <summary>
				/// Sets Caller ID
				/// </summary>
				/// <param name="callerID">Caller ID / Phone Number</param>
				/// <returns>VoiceBuilder</returns>
				public VoiceBuilder SetCallerID(string callerID)
        {
            Entity.CallerID = callerID;

            return this;
        }

        public VoiceBuilder SetOptions(string options)
        {
            Entity.Options = options;

            return this;
        }

        /// <summary>
        /// Sets number of operators (no of concurrent calls)
        /// </summary>
        /// <param name="numberOfOperators">Number of operators</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetNumberOfOperators(int numberOfOperators)
        {
            Entity.NumberOfOperators = numberOfOperators;

            return this;
        }

        /// <summary>
        /// Sets number of retry attempts
        /// </summary>
        /// <param name="retryAttempts">No. of attempts</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetRetryAttempts(int retryAttempts)
        {
            Entity.RetryAttempts = retryAttempts;

            return this;
        }

        /// <summary>
        /// Sets minutes between retries
        /// </summary>
        /// <param name="minutes">No. of minutes</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetRetryPeriod(int minutes)
        {
            Entity.RetryPeriod = minutes;

            return this;
        }

        /// <summary>
        /// Sets message to people (main message)
        /// </summary>
        /// <param name="fileLocation">Message file to play</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetMessageToPeople(string fileLocation)
        {
            Entity.MessageData.Add(MessageDataType.MessageToPeople, fileLocation);

            return this;
        }

        /// <summary>
        /// Sets message to answer phones
        /// </summary>
        /// <param name="fileLocation">Message file to play</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetMessageToAnswerPhones(string fileLocation)
        {
            Entity.MessageData.Add(MessageDataType.MessageToAnswerPhones, fileLocation);

            return this;
        }

        /// <summary>
        /// Sets message to play when call routed (two-way call connected)
        /// </summary>
        /// <param name="fileLocation">Message file to play</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetCallRouteMessageToPeople(string fileLocation)
        {
            Entity.MessageData.Add(MessageDataType.CallRouteMessageToPeople, fileLocation);

            return this;
        }

        /// <summary>
        /// Sets message to operators when call routed (two-way call connected)
        /// </summary>
        /// <param name="fileLocation">Message file to play</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetCallRouteMessageToOperators(string fileLocation)
        {
            Entity.MessageData.Add(MessageDataType.CallRouteMessageToOperators, fileLocation);

            return this;
        }

        /// <summary>
        /// Sets message to play when wrong key pressed
        /// </summary>
        /// <param name="fileLocation">Message file to play</param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder SetCallRouteMessageOnWrongKey(string fileLocation)
        {
            Entity.MessageData.Add(MessageDataType.CallRouteMessageOnWrongKey, fileLocation);

            return this;
        }

        #endregion

        #region AddMessageData
        /// <summary>
        /// Adding messages (wave files)
        /// </summary>
        /// <param name="messageDataType">Type of message - enum Voice.MessageDataType</param>
        /// <param name="fileLocation">File location of your recorded message (wave format)</param>
        /// <returns></returns>
        public void AddMessageData(MessageDataType messageDataType, string fileLocation)
        {
            Entity.MessageData.Add(messageDataType, fileLocation);
        }

        #endregion AddMessageData

        #region Add Recipients
        /// <summary>
        /// Adding recipient
        /// </summary>
        /// <param name="recipient">Fax number</param>
        /// <returns>TTSBuilder</returns>
        public VoiceBuilder AddRecipient(string recipient)
        {
            Recipients.Add(new Recipient()
            {
                PhoneNumber = recipient
            });

            return this;
        }

        /// <summary>
        /// Adding recipient object
        /// </summary>
        /// <param name="recipient">Recipient object</param>
        /// <returns>TTSBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipient(Recipient recipient)
        {
            Recipients.Add(recipient);

            return this;
        }

        /// <summary>
        /// Adding list of Recipient
        /// </summary>
        /// <param name="recipients">Recipient collection</param>
        /// <returns>TTSBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Recipients.Add(recipients);

            return this;
        }

        /// <summary>
        /// Adding list of Recipient
        /// </summary>
        /// <param name="recipients">Fax number list</param>
        /// <returns>TTSBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipients(ICollection<string> recipients)
        {
            foreach (var recipient in recipients)
            {
                AddRecipient(recipient);
            }

            return this;
        }

        #endregion Add Recipients

        #region Add Recipients using TNZ Addressbook

        /// <summary>
        /// Adding recipient using ContactModel (TNZ Addressbook)
        /// </summary>
        /// <param name="contact">ContactModel</param>
        /// <returns>VoiceBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipient(ContactModel contact)
        {
            Recipients.Add(contact);

            return this;
        }

        /// <summary>
        /// Adding recipient using ContactID (TNZ Addressbook)
        /// </summary>
        /// <param name="contactID">ContactID</param>
        /// <returns>VoiceBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipient(ContactID contactID)
        {
            Recipients.Add(contactID);

            return this;
        }

        /// <summary>
        /// Adding recipients using GroupModel (TNZ Addressbook)
        /// </summary>
        /// <param name="group">GroupModel</param>
        /// <returns>VoiceBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipients(GroupModel group)
        {
            Recipients.Add(group);

            return this;
        }

        /// <summary>
        /// Adding recipients using GroupID (TNZ Addressbook)
        /// </summary>
        /// <param name="groupID">GroupID</param>
        /// <returns>VoiceBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipients(GroupID groupID)
        {
            Recipients.Add(groupID);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of ContactModels (TNZ Addressbook)
        /// </summary>
        /// <param name="contacts">ICollection<ContactModel></param>
        /// <returns>VoiceBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipients(ICollection<ContactModel> contacts)
        {
            Recipients.Add(contacts);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of ContactIDs (TNZ Addressbook)
        /// </summary>
        /// <param name="contactIDs">ICollection<ContactID></param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder AddRecipients(ICollection<ContactID> contactIDs)
        {
            Recipients.Add(contactIDs);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of GroupModels (TNZ Addressbook)
        /// </summary>
        /// <param name="groups">ICollection<GroupModel></param>
        /// <returns>VoiceBuilder</returns>
        [ComVisible(false)]
        public VoiceBuilder AddRecipients(ICollection<GroupModel> groups)
        {
            Recipients.Add(groups);

            return this;
        }

        /// <summary>
        /// Adding recipients using list of GroupIDs (TNZ Addressbook)
        /// </summary>
        /// <param name="groupIDs">ICollection<GroupID></param>
        /// <returns>VoiceBuilder</returns>
        public VoiceBuilder AddRecipients(ICollection<GroupID> groupIDs)
        {
            Recipients.Add(groupIDs);

            return this;
        }

        #endregion Add Recipients using TNZ Addressbook

        #region Build / BuildAsync
        /// <summary>
        /// Build TTS Message
        /// </summary>
        /// <returns>SMSModel</returns>
        public VoiceModel Build()
        {
            Entity.Recipients = Recipients.ToList();

            foreach (var data in Entity.MessageData)
            {
                using (var attachment = Attachment.GetAttachment(data.Value))
                {
                    Entity.MessageDataAttachments.Add(data.Key, attachment);
                }
            }

            return Entity;
        }

        /// <summary>
        /// Build TTS Message Async
        /// </summary>
        /// <returns>IMessage</returns>
        public async Task<VoiceModel> BuildAsync()
        {
            Entity.Recipients = Recipients.ToList();

            foreach (var data in Entity.MessageData)
            {
                using (var attachment = await Attachment.GetAttachmentAsync(data.Value))
                {
                    Entity.MessageDataAttachments.Add(data.Key, attachment);
                }
            }

            return Entity;
        }

        #endregion Build / BuildAsync
    }
}
