using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Api.Messaging.TTS.Dto;
using static TNZAPI.NET.Core.Enums;

namespace TNZAPI.NET.Api.Messaging.TTS
{
	public class TTSBuilder : IDisposable
    {
        private TTSModel Entity { get; set; }

        private RecipientList Recipients { get; set; }

        private KeypadList Keypads { get; set; }

        /// <summary>
        /// Builder builds TTS message to send through TNZAPI
        /// </summary>
        public TTSBuilder()
        {
            Entity = new TTSModel();
            Recipients = new RecipientList();
            Keypads = new KeypadList();
        }

        /// <summary>
        /// Dispose TTSBuilder
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
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetErrorEmailNotify(string emailAddress)
        {
            Entity.ErrorEmailNotify = emailAddress;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback URL to receive webhooks
        /// </summary>
        /// <param name="url">Your URL to receive webhooks</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback Format - JSON/XML
        /// </summary>
        /// <param name="type">WebhookCallbackType</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetWebhookCallbackFormat(WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        /// <summary>
        /// Sets Send mode - Live/Text
        /// </summary>
        /// <param name="mode">SendModeType</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetSendMode(SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        /// <summary>
        /// Sets MessageID of the email
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = new MessageID(messageID);

            return this;
        }

		/// <summary>
		/// Sets MessageID of the message
		/// </summary>
		/// <param name="messageID">MessageID</param>
		/// <returns>TTSBuilder</returns>
		public TTSBuilder SetMessageID(MessageID messageID)
		{
			Entity.MessageID = messageID;

			return this;
		}

		/// <summary>
		/// Sets Reference of the message
		/// </summary>
		/// <param name="reference"></param>
		/// <returns>TTSBuilder</returns>
		public TTSBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        /// <summary>
        /// Sets Send Time of the message
        /// </summary>
        /// <param name="TTSBuilder">DateTime</param>
        /// <returns>EmailBuilder</returns>
        public TTSBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        /// <summary>
        /// Sets Timezone with SendTime
        /// </summary>
        /// <param name="timezone">string</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        /// <summary>
        /// Sets SubAccount value of the message
        /// </summary>
        /// <param name="subaccount">SubAccount value</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        /// <summary>
        /// Sets Department value of the message
        /// </summary>
        /// <param name="department">Department value</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        /// <summary>
        /// Sets ChargeCode value of the message
        /// </summary>
        /// <param name="chargeCode">Charge Code Value</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }

        /// <summary>
        /// Adding messages
        /// </summary>
        /// <param name="messageDataType">Type of message - enum Voice.MessageDataType</param>
        /// <param name="ttsMessage">Message to be sent (Text format)</param>
        /// <returns></returns>
        public TTSBuilder SetMessageData(MessageDataType messageDataType, string ttsMessage)
        {
            switch (messageDataType)
            {
                case MessageDataType.MessageToPeople:
                    Entity.MessageToPeople = ttsMessage;
                    break;
                case MessageDataType.MessageToAnswerPhones:
                    Entity.MessageToAnswerPhones = ttsMessage;
                    break;
                case MessageDataType.CallRouteMessageToPeople:
                    Entity.CallRouteMessageToPeople = ttsMessage;
                    break;
                case MessageDataType.CallRouteMessageToOperators:
                    Entity.CallRouteMessageToOperators = ttsMessage;
                    break;
                case MessageDataType.CallRouteMessageOnWrongKey:
                    Entity.CallRouteMessageOnWrongKey = ttsMessage;
                    break;
            }

            return this;
        }

        #endregion

        #region TTS Specific

        /// <summary>
        /// Sets Caller ID
        /// </summary>
        /// <param name="callerID">Caller ID / Phone Number</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetCallerID(string callerID)
        {
            Entity.CallerID = callerID;

            return this;
        }

        /// <summary>
        /// Sets TTS Voice Type
        /// </summary>
        /// <param name="ttsVoiceType">TTSVoiceType</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetTTSVoice(TTSVoiceType ttsVoiceType)
        {
            Entity.TTSVoice = ttsVoiceType;

            return this;
        }

        /// <summary>
        /// Sets report to email address
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetReportTo(string emailAddress)
        {
            Entity.ReportTo = emailAddress;

            return this;
        }

        public TTSBuilder SetOptions(string options)
        {
            Entity.Options = options;

            return this;
        }

        /// <summary>
        /// Sets number of operators (no of concurrent calls)
        /// </summary>
        /// <param name="numberOfOperators">Number of operators</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetNumberOfOperators(int numberOfOperators)
        {
            Entity.NumberOfOperators = numberOfOperators;

            return this;
        }

        /// <summary>
        /// Sets number of retry attempts
        /// </summary>
        /// <param name="retryAttempts">No. of attempts</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetRetryAttempts(int retryAttempts)
        {
            Entity.RetryAttempts = retryAttempts;

            return this;
        }

        /// <summary>
        /// Sets minutes between retries
        /// </summary>
        /// <param name="minutes">No. of minutes</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetRetryPeriod(int minutes)
        {
            Entity.RetryPeriod = minutes;

            return this;
        }

        /// <summary>
        /// Sets message to people (main message)
        /// </summary>
        /// <param name="message">Message to play</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetMessageToPeople(string message)
        {
            Entity.MessageToPeople = message;

            return this;
        }

        /// <summary>
        /// Sets message to answer phones
        /// </summary>
        /// <param name="message">Message to play</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetMessageToAnswerPhones(string message)
        {
            Entity.MessageToAnswerPhones = message;

            return this;
        }

        /// <summary>
        /// Sets message to play when call routed (two-way call connected)
        /// </summary>
        /// <param name="message">Message to play</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetCallRouteMessageToPeople(string message)
        {
            Entity.CallRouteMessageToPeople = message;

            return this;
        }

        /// <summary>
        /// Sets message to operators when call routed (two-way call connected)
        /// </summary>
        /// <param name="message">Message to play</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetCallRouteMessageToOperators(string message)
        {
            Entity.CallRouteMessageToOperators = message;

            return this;
        }

        /// <summary>
        /// Sets message to play when wrong key pressed
        /// </summary>
        /// <param name="message">Message to play</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder SetCallRouteMessageOnWrongKey(string message)
        {
            Entity.CallRouteMessageOnWrongKey = message;

            return this;
        }

        #endregion

        #region Add Recipients
        /// <summary>
        /// Adding recipient
        /// </summary>
        /// <param name="recipient">Fax number</param>
        /// <returns>TTSBuilder</returns>
        public TTSBuilder AddRecipient(string recipient)
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
        public TTSBuilder AddRecipient(Recipient recipient)
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
        public TTSBuilder AddRecipients(ICollection<Recipient> recipients)
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
        public TTSBuilder AddRecipients(ICollection<string> recipients)
        {
            foreach (var recipient in recipients)
            {
                AddRecipient(recipient);
            }

            return this;
        }

        #endregion Add Recipients

        #region Build / BuildAsync

        /// <summary>
        /// Build TTS Message
        /// </summary>
        /// <returns>SMSModel</returns>
        public TTSModel Build()
        {
            Entity.Recipients = Recipients.ToList();

            return Entity;
        }

        /// <summary>
        /// Build TTS Message Async
        /// </summary>
        /// <returns>IMessage</returns>
        public async Task<TTSModel> BuildAsync()
        {
            return await Task.Run(() =>
            {
                Entity.Recipients = Recipients.ToList();

                return Entity;
            });
        }
        #endregion Build / BuildAsync
    }
}
