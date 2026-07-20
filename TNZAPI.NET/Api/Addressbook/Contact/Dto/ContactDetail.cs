using TNZAPI.NET.Api.Messaging.Common.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Addressbook.Contact.Dto
{
    public class ContactDetail
    {
        public ContactID? ContactID { get; set; }
        public string? Owner { get; set; }
        public DateTime? CreatedTimeLocal { get; set; }
        public DateTime? CreatedTimeUTC { get; set; }
        public DateTime? CreatedTimeUTC_RFC3339 { get; set; }
        public DateTime? UpdatedTimeLocal { get; set; }
        public DateTime? UpdatedTimeUTC { get; set; }
        public DateTime? UpdatedTimeUTC_RFC3339 { get; set; }
        public string? Timezone { get; set; }

        public string? ExType { get; set; }
        public string? ExID { get; set; }
        public Enums.ViewEditByOptions? ViewBy { get; set; }
        public Enums.ViewEditByOptions? EditBy { get; set; }
        public Enums.AccessControlLevel? AccessControl { get; set; }
        public string? Attention { get; set; }
        public string? Title { get; set; }
        public string? Company { get; set; }
        public string? RecipDepartment { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public string? StreetAddress { get; set; }
        public string? Suburb { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Postcode { get; set; }
        public string? MainPhone { get; set; }
        public string? DirectPhone { get; set; }
        public string? AltPhone1 { get; set; }
        public string? AltPhone2 { get; set; }
        public string? AltPhone3 { get; set; }
        public string? AltPhone4 { get; set; }
        public string? AltPhone5 { get; set; }
        public string? AltPhone6 { get; set; }
        public string? AltPhone7 { get; set; }
        public string? AltPhone8 { get; set; }
        public string? MobilePhone { get; set; }
        public string? FaxNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? WebAddress { get; set; }
        public string? Custom1 { get; set; }
        public string? Custom2 { get; set; }
        public string? Custom3 { get; set; }
        public string? Custom4 { get; set; }
        public string? Notes { get; set; }
    }
}