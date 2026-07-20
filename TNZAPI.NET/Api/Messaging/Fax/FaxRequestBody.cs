using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Messaging.Fax
{
    internal class FaxRequestBody
    {
        public MessageID? MessageID { get; set; }
        public string? Reference { get; set; }
        public string? TemplateID { get; set; }
        public ContactID? ContactID { get; set; }
        public GroupID? GroupID { get; set; }
        public List<FaxDestinationBody> Destinations { get; set; } = new List<FaxDestinationBody>();
        public List<FaxFileBody> Files { get; set; } = new List<FaxFileBody>();
        public Enums.NotificationType? NotificationType { get; set; }
        public string? WebhookCallbackURL { get; set; }
        public Enums.WebhookCallbackType? WebhookCallbackFormat { get; set; }
        public string? ReportTo { get; set; }
        public DateTime? SendTime { get; set; }
        public string? Timezone { get; set; }
        public string? SubAccount { get; set; }
        public string? Department { get; set; }
        public string? ChargeCode { get; set; }
        public string? CSID { get; set; }
        public Enums.FaxResolution? Resolution { get; set; }
        public string? WatermarkFolder { get; set; }
        public string? WatermarkFirstPage { get; set; }
        public string? WatermarkAllPages { get; set; }
        public int? RetryAttempts { get; set; }
        public int? RetryPeriod { get; set; }
        public string? Mode { get; set; }
    }

    internal class FaxDestinationBody
    {
        public string? Recipient { get; set; }
        public string? ToNumber { get; set; }
        public string? MobilePhone { get; set; }
        public string? MainPhone { get; set; }
        public string? FaxNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Attention { get; set; }
        public string? Company { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Custom1 { get; set; }
        public string? Custom2 { get; set; }
        public string? Custom3 { get; set; }
        public string? Custom4 { get; set; }
        public string? Custom5 { get; set; }
        public string? Custom6 { get; set; }
        public string? Custom7 { get; set; }
        public string? Custom8 { get; set; }
        public string? Custom9 { get; set; }
        public ContactID? ContactID { get; set; }
        public GroupID? GroupID { get; set; }
        public string? GroupCode { get; set; }
    }

    internal class FaxFileBody
    {
        public string? Name { get; set; }
        public string? Data { get; set; }
    }
}