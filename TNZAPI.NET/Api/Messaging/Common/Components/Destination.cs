using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Api.Messaging.Common.Components
{
    // Mirrors TNZ's own "Destinations Object" wire shape 1:1 (v3.00 REST API docs), unlike Recipient
    // (which only ever maps onto one channel-specific "alternative" address field per module —
    // ToNumber/MainPhone/EmailAddress). Recipient is the primary, documented field ("Primary
    // recipient address/number") — a single channel-agnostic address, distinct from the
    // channel-specific alternative fields that also live on the same wire object. Deliberately kept
    // independent from Recipient (not a subtype/wrapper) so each type's wire mapping stays simple and
    // neither is constrained by the other's shape.
    public class Destination
    {
        public string? Recipient { get; set; }
        public string? ToNumber { get; set; }
        public string? MobilePhone { get; set; }
        public string? MainPhone { get; set; }
        public string? FaxNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Attention { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Company { get; set; }
        public string? Custom1 { get; set; }
        public string? Custom2 { get; set; }
        public string? Custom3 { get; set; }
        public string? Custom4 { get; set; }
        public string? Custom5 { get; set; }
        public string? Custom6 { get; set; }
        public string? Custom7 { get; set; }
        public string? Custom8 { get; set; }
        public string? Custom9 { get; set; }
        public GroupID? GroupID { get; set; }
        public string? GroupCode { get; set; }
        public ContactID? ContactID { get; set; }

        public Destination()
        {
        }

        public Destination(string recipient)
        {
            Recipient = recipient;
        }

        public Destination(GroupID groupID)
        {
            GroupID = groupID;
        }

        public Destination(ContactID contactID)
        {
            ContactID = contactID;
        }

        public Destination(string? recipient = null, string? companyName = null, string? attention = null, string? firstName = null, string? lastName = null, string? custom1 = null, string? custom2 = null, string? custom3 = null, string? custom4 = null, string? custom5 = null, string? custom6 = null, string? custom7 = null, string? custom8 = null, string? custom9 = null)
        {
            if (recipient is not null)
            {
                Recipient = recipient;
            }

            if (companyName is not null)
            {
                Company = companyName;
            }
            if (attention is not null)
            {
                Attention = attention;
            }
            if (firstName is not null)
            {
                FirstName = firstName;
            }
            if (lastName is not null)
            {
                LastName = lastName;
            }

            if (custom1 is not null)
            {
                Custom1 = custom1;
            }
            if (custom2 is not null)
            {
                Custom2 = custom2;
            }
            if (custom3 is not null)
            {
                Custom3 = custom3;
            }
            if (custom4 is not null)
            {
                Custom4 = custom4;
            }
            if (custom5 is not null)
            {
                Custom5 = custom5;
            }
            if (custom6 is not null)
            {
                Custom6 = custom6;
            }
            if (custom7 is not null)
            {
                Custom7 = custom7;
            }
            if (custom8 is not null)
            {
                Custom8 = custom8;
            }
            if (custom9 is not null)
            {
                Custom9 = custom9;
            }
        }
    }
}