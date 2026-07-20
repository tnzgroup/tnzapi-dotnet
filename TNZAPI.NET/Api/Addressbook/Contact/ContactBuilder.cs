using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Api.Addressbook.Contact
{
    public sealed class ContactBuilder : IDisposable
    {
        private ContactModel Entity { get; set; }

        public ContactBuilder()
        {
            Entity = new ContactModel();
        }

        public void Dispose()
        {
            Entity = new ContactModel();
        }

        public ContactBuilder SetExType(string exType)
        {
            Entity.ExType = exType;

            return this;
        }

        public ContactBuilder SetExID(string exID)
        {
            Entity.ExID = exID;

            return this;
        }

        public ContactBuilder SetViewBy(Enums.ViewEditByOptions viewBy)
        {
            Entity.ViewBy = viewBy;

            return this;
        }

        public ContactBuilder SetEditBy(Enums.ViewEditByOptions editBy)
        {
            Entity.EditBy = editBy;

            return this;
        }

        public ContactBuilder SetAccessControl(Enums.AccessControlLevel accessControl)
        {
            Entity.AccessControl = accessControl;

            return this;
        }

        public ContactBuilder SetAttention(string attention)
        {
            Entity.Attention = attention;

            return this;
        }

        public ContactBuilder SetTitle(string title)
        {
            Entity.Title = title;

            return this;
        }

        public ContactBuilder SetCompany(string company)
        {
            Entity.Company = company;

            return this;
        }

        public ContactBuilder SetRecipDepartment(string recipDepartment)
        {
            Entity.RecipDepartment = recipDepartment;

            return this;
        }

        public ContactBuilder SetFirstName(string firstName)
        {
            Entity.FirstName = firstName;

            return this;
        }

        public ContactBuilder SetLastName(string lastName)
        {
            Entity.LastName = lastName;

            return this;
        }

        public ContactBuilder SetPosition(string position)
        {
            Entity.Position = position;

            return this;
        }

        public ContactBuilder SetStreetAddress(string streetAddress)
        {
            Entity.StreetAddress = streetAddress;

            return this;
        }

        public ContactBuilder SetSuburb(string suburb)
        {
            Entity.Suburb = suburb;

            return this;
        }

        public ContactBuilder SetCity(string city)
        {
            Entity.City = city;

            return this;
        }

        public ContactBuilder SetState(string state)
        {
            Entity.State = state;

            return this;
        }

        public ContactBuilder SetCountry(string country)
        {
            Entity.Country = country;

            return this;
        }

        public ContactBuilder SetPostcode(string postcode)
        {
            Entity.Postcode = postcode;

            return this;
        }

        public ContactBuilder SetMainPhone(string mainPhone)
        {
            Entity.MainPhone = mainPhone;

            return this;
        }

        public ContactBuilder SetDirectPhone(string directPhone)
        {
            Entity.DirectPhone = directPhone;

            return this;
        }

        public ContactBuilder SetAltPhone1(string altPhone1)
        {
            Entity.AltPhone1 = altPhone1;

            return this;
        }

        public ContactBuilder SetAltPhone2(string altPhone2)
        {
            Entity.AltPhone2 = altPhone2;

            return this;
        }

        public ContactBuilder SetAltPhone3(string altPhone3)
        {
            Entity.AltPhone3 = altPhone3;

            return this;
        }

        public ContactBuilder SetAltPhone4(string altPhone4)
        {
            Entity.AltPhone4 = altPhone4;

            return this;
        }

        public ContactBuilder SetAltPhone5(string altPhone5)
        {
            Entity.AltPhone5 = altPhone5;

            return this;
        }

        public ContactBuilder SetAltPhone6(string altPhone6)
        {
            Entity.AltPhone6 = altPhone6;

            return this;
        }

        public ContactBuilder SetAltPhone7(string altPhone7)
        {
            Entity.AltPhone7 = altPhone7;

            return this;
        }

        public ContactBuilder SetAltPhone8(string altPhone8)
        {
            Entity.AltPhone8 = altPhone8;

            return this;
        }

        public ContactBuilder SetMobilePhone(string mobilePhone)
        {
            Entity.MobilePhone = mobilePhone;

            return this;
        }

        public ContactBuilder SetFaxNumber(string faxNumber)
        {
            Entity.FaxNumber = faxNumber;

            return this;
        }

        public ContactBuilder SetEmailAddress(string emailAddress)
        {
            Entity.EmailAddress = emailAddress;

            return this;
        }

        public ContactBuilder SetWebAddress(string webAddress)
        {
            Entity.WebAddress = webAddress;

            return this;
        }

        public ContactBuilder SetCustom1(string custom1)
        {
            Entity.Custom1 = custom1;

            return this;
        }

        public ContactBuilder SetCustom2(string custom2)
        {
            Entity.Custom2 = custom2;

            return this;
        }

        public ContactBuilder SetCustom3(string custom3)
        {
            Entity.Custom3 = custom3;

            return this;
        }

        public ContactBuilder SetCustom4(string custom4)
        {
            Entity.Custom4 = custom4;

            return this;
        }

        public ContactBuilder SetNotes(string notes)
        {
            Entity.Notes = notes;

            return this;
        }

        public ContactModel Build()
        {
            return Entity;
        }
    }
}