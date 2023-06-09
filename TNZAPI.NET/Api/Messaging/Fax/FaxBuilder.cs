using System.Linq.Expressions;
using System.Runtime.InteropServices;
using TNZAPI.NET.Api.Messaging.Common.Components;
using TNZAPI.NET.Api.Messaging.Common.Components.List;
using TNZAPI.NET.Api.Messaging.Fax.Dto;
using TNZAPI.NET.Helpers;
using static TNZAPI.NET.Api.Messaging.Common.Enums;

namespace TNZAPI.NET.Api.Messaging.Fax
{
    public sealed class FaxBuilder : IDisposable
    {
        private FaxModel Entity { get; set; }

        private RecipientList Recipients { get; set; }

        private AttachmentList Attachments { get; set; }

        /// <summary>
        /// Builder builds fax message to send through TNZAPI
        /// </summary>
        public FaxBuilder()
        {
            Entity = new FaxModel();
            Recipients = new RecipientList();
            Attachments = new AttachmentList();
        }

        /// <summary>
        /// Dispose Builder
        /// </summary>
        public void Dispose()
        {
            Entity = null;
            Recipients = null;
            Attachments = null;
        }

        #region General

        /// <summary>
        /// Sets ErrorEmailNotify, email address to get error notifications
        /// </summary>
        /// <param name="emailAddress">Your email address</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetErrorEmailNotify(string emailAddress)
        {
            Entity.ErrorEmailNotify = emailAddress;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback URL to receive webhooks
        /// </summary>
        /// <param name="url">Your URL to receive webhooks</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetWebhookCallbackURL(string url)
        {
            Entity.WebhookCallbackURL = url;

            return this;
        }

        /// <summary>
        /// Sets Webhook Callback Format - JSON/XML
        /// </summary>
        /// <param name="type">WebhookCallbackType</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetWebhookCallbackFormat(WebhookCallbackType type)
        {
            Entity.WebhookCallbackFormat = type;

            return this;
        }

        /// <summary>
        /// Sets Send mode - Live/Text
        /// </summary>
        /// <param name="mode">SendModeType</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetSendMode(SendModeType mode)
        {
            Entity.SendMode = mode;

            return this;
        }

        /// <summary>
        /// Sets MessageID of the email
        /// </summary>
        /// <param name="messageID">Message ID</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetMessageID(string messageID)
        {
            Entity.MessageID = messageID;

            return this;
        }

        /// <summary>
        /// Sets Reference of the email
        /// </summary>
        /// <param name="reference"></param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetReference(string reference)
        {
            Entity.Reference = reference;

            return this;
        }

        /// <summary>
        /// Sets Send Time of the email
        /// </summary>
        /// <param name="sendTime">DateTime</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetSendTime(DateTime sendTime)
        {
            Entity.SendTime = sendTime;

            return this;
        }

        /// <summary>
        /// Sets Timezone with SendTime
        /// </summary>
        /// <param name="timezone">string</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetTimezone(string timezone)
        {
            Entity.Timezone = timezone;

            return this;
        }

        /// <summary>
        /// Sets SubAccount value of the email
        /// </summary>
        /// <param name="subaccount">SubAccount value</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetSubAccount(string subaccount)
        {
            Entity.SubAccount = subaccount;

            return this;
        }

        /// <summary>
        /// Sets Department value of the email
        /// </summary>
        /// <param name="department">Department value</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetDepartment(string department)
        {
            Entity.Department = department;

            return this;
        }

        /// <summary>
        /// Sets ChargeCode value of the email
        /// </summary>
        /// <param name="chargeCode">Charge Code Value</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetChargeCode(string chargeCode)
        {
            Entity.ChargeCode = chargeCode;

            return this;
        }

        #endregion

        #region Fax Specific

        /// <summary>
        /// Sets fax resolution
        /// </summary>
        /// <param name="resolution">HIGH/LOW</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetResolution(string resolution)
        {
            Entity.Resolution = resolution;

            return this;
        }

        /// <summary>
        /// Sets CSID
        /// </summary>
        /// <param name="csid">CSID</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetCSID(string csid)
        {
            Entity.CSID = csid;

            return this;
        }

        /// <summary>
        /// Sets Stamp format
        /// </summary>
        /// <param name="stampFormat"></param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetStampFormat(string stampFormat)
        {
            Entity.StampFormat = stampFormat;

            return this;
        }

        /// <summary>
        /// Sets location of watermark folder (must exists in TNZ)
        /// </summary>
        /// <param name="watermarkFolder">Folder path</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetWatermarkFolder(string watermarkFolder)
        {
            Entity.WatermarkFolder = watermarkFolder;

            return this;
        }

        public FaxBuilder SetWatermarkFirstPage(string yesno)
        {
            Entity.WatermarkFirstPage = yesno.ToUpper() == "YES" ? "Yes" : "No";

            return this;
        }

        public FaxBuilder SetWatermarkAllPages(string yesno)
        {
            Entity.WatermarkAllPages = yesno.ToUpper() == "YES" ? "Yes" : "No";

            return this;
        }

        /// <summary>
        /// Sets no. of retry attempts
        /// </summary>
        /// <param name="retryAttempts">no. of retry attemtps</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetRetryAttempts(int retryAttempts)
        {
            Entity.RetryAttempts = retryAttempts;

            return this;
        }

        /// <summary>
        /// Sets no. of minutes between retries
        /// </summary>
        /// <param name="minutes">Minutes</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder SetRetryPeriod(int minutes)
        {
            Entity.RetryPeriod = minutes;

            return this;
        }

        #endregion

        #region Add Recipients
        /// <summary>
        /// Adding recipient
        /// </summary>
        /// <param name="recipient">Fax number</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder AddRecipient(string recipient)
        {
            Recipients.Add(new Recipient()
            {
                FaxNumber = recipient
            });

            return this;
        }

        /// <summary>
        /// Adding recipient object
        /// </summary>
        /// <param name="recipient">Recipient object</param>
        /// <returns>FaxBuilder</returns>
        [ComVisible(false)]
        public FaxBuilder AddRecipient(Recipient recipient)
        {
            Recipients.Add(recipient);

            return this;
        }

        /// <summary>
        /// Adding list of Recipient
        /// </summary>
        /// <param name="recipients">Recipient collection</param>
        /// <returns>FaxBuilder</returns>
        [ComVisible(false)]
        public FaxBuilder AddRecipients(ICollection<Recipient> recipients)
        {
            Recipients.Add(recipients);

            return this;
        }

        /// <summary>
        /// Adding list of Recipient
        /// </summary>
        /// <param name="recipients">Fax number list</param>
        /// <returns>FaxBuilder</returns>
        [ComVisible(false)]
        public FaxBuilder AddRecipients(ICollection<string> recipients)
        {
            foreach (var recipient in recipients)
            {
                AddRecipient(recipient);
            }

            return this;
        }

        #endregion Add Recipients

        #region AddAttachment
        /// <summary>
        /// Adding attachment
        /// </summary>
        /// <param name="fileLocation">File location</param>
        /// <returns>FaxBuilder</returns>
        public FaxBuilder AddAttachment(string fileLocation)
        {
            Attachments.Add(fileLocation);

            return this;
        }

        /// <summary>
        /// Adding attachment
        /// </summary>
        /// <param name="attachment">Attachment for your fax</param>
        /// <returns>FaxBuilder</returns>
        [ComVisible(false)]
        public FaxBuilder AddAttachment(Attachment attachment)
        {
            Attachments.Add(attachment);

            return this;
        }

        /// <summary>
        /// Adding attachment)
        /// </summary>
        /// <param name="fileLocations">List of location of your attachments</param>
        /// <returns>FaxBuilder</returns>
        [ComVisible(false)]
        public FaxBuilder AddAttachments(ICollection<string> fileLocations)
        {
            Attachments.Add(fileLocations);

            return this;
        }

        #endregion AddAttachment

        #region Set

        public void Set<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<FaxModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, FaxModel>(propertyExpression);
            PropertyHelper.SetProperty(Entity, convertedExpression, value);
        }

        #endregion

        #region Build / BuildAsync

        /// <summary>
        /// Build fax message
        /// </summary>
        /// <returns>FaxModel</returns>
        public FaxModel Build()
        {
            Entity.Recipients = Recipients.ToList();
            Entity.Attachments = Attachments.ToList();

            return Entity;
        }

        /// <summary>
        /// Build fax message (async)
        /// </summary>
        /// <returns>FaxModel</returns>
        public async Task<FaxModel> BuildAsync()
        {
            Entity.Recipients = Recipients.ToList();
            Entity.Attachments = await Attachments.ToListAsync();

            return Entity;
        }
        #endregion Build / BuildAsync
    }
}
