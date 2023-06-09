using System.Linq.Expressions;
using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Addressbook.Contact
{
    public class ContactBuilder : IDisposable
    {
        private ContactModel Entity { get; set; }

        public ContactBuilder()
        {
            Entity = new ContactModel();
        }

        #region Dispose

        public void Dispose()
        {
            Entity = null;
        }

        #endregion

        #region Options
        /// <summary>
        /// Sets the Attention
        /// </summary>
        /// <param name="attention">Attention</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetAttention(string attention)
        {
            Entity.Attention = attention;

            return this;
        }

        /// <summary>
        /// Sets the company name
        /// </summary>
        /// <param name="company">Company name</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetCompany(string company)
        {
            Entity.Company = company;

            return this;
        }

        /// <summary>
        /// Sets the company department
        /// </summary>
        /// <param name="companyDepartment">Company Department</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetCompanyDepartment(string companyDepartment)
        {
            Entity.CompanyDepartment = companyDepartment;

            return this;
        }

        /// <summary>
        /// Sets the first name
        /// </summary>
        /// <param name="firstName">First Name</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetFirstName(string firstName)
        {
            Entity.FirstName = firstName;

            return this;
        }

        /// <summary>
        /// Sets the last name
        /// </summary>
        /// <param name="lastName">Last Name</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetLastName(string lastName)
        {
            Entity.LastName = lastName;

            return this;
        }

        /// <summary>
        /// Sets the position
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetPosition(string position)
        {
            Entity.Position = position;

            return this;
        }

        /// <summary>
        /// Sets the street address
        /// </summary>
        /// <param name="streetAddress">Street Address</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetStreetAddress(string streetAddress)
        {
            Entity.StreetAddress = streetAddress;

            return this;
        }

        /// <summary>
        /// Sets the suburb
        /// </summary>
        /// <param name="suburb">Suburb</param>
        /// <returns></returns>
        public ContactBuilder SetSuburb(string suburb)
        {
            Entity.Suburb = suburb;

            return this;
        }

        /// <summary>
        /// Sets the city
        /// </summary>
        /// <param name="city">City</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetCity(string city)
        {
            Entity.City = city;

            return this;
        }

        /// <summary>
        /// Sets the state (Contact's Address)
        /// </summary>
        /// <param name="state">State</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetState(string state)
        {
            Entity.State = state;

            return this;
        }

        /// <summary>
        /// Sets the country
        /// </summary>
        /// <param name="country">Country</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetCountry(string country)
        {
            Entity.Country = country;

            return this;
        }

        /// <summary>
        /// Sets the post code
        /// </summary>
        /// <param name="postCode">Post Code</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetPostCode(string postCode)
        {
            Entity.Postcode = postCode;

            return this;
        }

        /// <summary>
        /// Set Main Phone - for TTS/Voice
        /// </summary>
        /// <param name="mainPhone">Phone number</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetMainPhone(string mainPhone)
        {
            Entity.MainPhone = mainPhone;

            return this;
        }

        /// <summary>
        /// Set Alternative Phone 1 - for TTS/Voice
        /// </summary>
        /// <param name="altPhone1">Phone number</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetAltPhone1(string altPhone1)
        {
            Entity.AltPhone1 = altPhone1;

            return this;
        }

        /// <summary>
        /// Set Alternative Phone 2 - for TTS/Voice
        /// </summary>
        /// <param name="altPhone2">Phone number</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetAltPhone2(string altPhone2)
        {
            Entity.AltPhone2 = altPhone2;

            return this;
        }

        /// <summary>
        /// Set DDI Number
        /// </summary>
        /// <param name="ddiNumber">DDI number</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetDDINumber(string ddiNumber)
        {
            Entity.DirectPhone = ddiNumber;

            return this;
        }

        /// <summary>
        /// Set mobile number - for sending SMS
        /// </summary>
        /// <param name="mobileNumber">Mobile number</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetMobileNumber(string mobileNumber)
        {
            Entity.MobilePhone = mobileNumber;

            return this;
        }

        /// <summary>
        /// Set fax number - for sending fax
        /// </summary>
        /// <param name="faxNumber">Fax number</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetFaxNumber(string faxNumber)
        {
            Entity.FaxNumber = faxNumber;

            return this;
        }

        /// <summary>
        /// Set email address - for sending email
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetEmailAddress(string emailAddress)
        {
            Entity.EmailAddress = emailAddress;

            return this;
        }

        /// <summary>
        /// Set customer's website
        /// </summary>
        /// <param name="webAddress">Website URL</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetWebAddress(string webAddress)
        {
            Entity.WebAddress = webAddress;

            return this;
        }

        /// <summary>
        /// Set custom 1
        /// </summary>
        /// <param name="custom1">Custom1</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetCustom1(string custom1)
        {
            Entity.Custom1 = custom1;

            return this;
        }

        /// <summary>
        /// Set custom 2
        /// </summary>
        /// <param name="custom2">Custom2</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetCustom2(string custom2)
        {
            Entity.Custom2 = custom2;

            return this;
        }

        /// <summary>
        /// Set custom 3
        /// </summary>
        /// <param name="custom3">Custom3</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetCustom3(string custom3)
        {
            Entity.Custom3 = custom3;

            return this;
        }

        /// <summary>
        /// Set custom 4
        /// </summary>
        /// <param name="custom4">Custom4</param>
        /// <returns>ContactBuilder</returns>
        public ContactBuilder SetCustom4(string custom4)
        {
            Entity.Custom4 = custom4;

            return this;
        }

        public void Set<T>(Expression<Func<T, object>> propertyExpression, object value)
        {
            Expression<Func<ContactModel, object>> convertedExpression = ExpressionHelper.ConvertExpressionParameterType<T, ContactModel>(propertyExpression);
            PropertyHelper.SetProperty(Entity, convertedExpression, value);
        }

        #endregion

        #region Build / BuildAsync

        /// <summary>
        /// Build ContactModel
        /// </summary>
        /// <returns>ContactModel</returns>
        public ContactModel Build()
        {
            return Entity;
        }

        /// <summary>
        /// Build ContactModel Async
        /// </summary>
        /// <returns>Task<ContactModel></returns>
        public async Task<ContactModel> BuildAsync()
        {
            await Task.Delay(0);

            return Entity;
        }
        #endregion Build / BuildAsync
    }
}
