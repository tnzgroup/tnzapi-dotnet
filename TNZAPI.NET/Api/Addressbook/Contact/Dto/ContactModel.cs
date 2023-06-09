﻿using System.Xml.Serialization;

namespace TNZAPI.NET.Api.Addressbook.Contact.Dto
{
    public class ContactModel
    {
        public string ID { get; set; }

        public string Owner { get; set; }

        // readonly
        public DateTime? Created { get; set; }
        // readonly
        public DateTime? Updated { get; set; }

        public string Attention { get; set; }
        public string Company { get; set; }

        [XmlElement("RecipDepartment")]
        public string CompanyDepartment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string MainPhone { get; set; }
        public string AltPhone1 { get; set; }
        public string AltPhone2 { get; set; }
        public string DirectPhone { get; set; }
        public string MobilePhone { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string WebAddress { get; set; }
        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
