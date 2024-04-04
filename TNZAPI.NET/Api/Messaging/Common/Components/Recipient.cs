using TNZAPI.NET.Api.Addressbook.Contact.Dto;

namespace TNZAPI.NET.Api.Messaging.Common.Components
{
    public class Recipient
    {
        public string CompanyName { get; set; }
        public string Attention { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Custom1 { get; set; }
        public string Custom2 { get; set; }
        public string Custom3 { get; set; }
        public string Custom4 { get; set; }
        public string Custom5 { get; set; }

        public GroupID GroupID { get; set; }
        public string GroupCode { get; set; }
        public ContactID ContactID { get; set; }

        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }


        /// <summary>
        /// Initiates Recipient
        /// </summary>
        /// <returns></returns>
        public Recipient()
        {
            CompanyName = "";
            Attention = "";
            FirstName = "";
            LastName = "";

            Custom1 = "";
            Custom2 = "";
            Custom3 = "";
            Custom4 = "";
            Custom5 = "";

            MobileNumber = "";
            FaxNumber = "";
            PhoneNumber = "";
            EmailAddress = "";
        }

        /// <summary>
        /// Initiates Recipient
        /// </summary>
        /// <param name="recipient">Recipient to receive message(s)</param>
        /// <returns></returns>
        public Recipient(string recipient)
        {
            MobileNumber = recipient;
            FaxNumber = recipient;
            PhoneNumber = recipient;
            EmailAddress = recipient;
        }

        /// <summary>
        /// Initiates Recipient
        /// </summary>
        /// <param name="recipient">Recipient to receive message(s)</param>
        /// <param name="companyName">Company Name - for substitutions</param>
        /// <param name="attention">Attention - for substitutions</param>
        /// <returns></returns>
        public Recipient(string recipient, string companyName, string attention)
        {
            MobileNumber = recipient;
            FaxNumber = recipient;
            PhoneNumber = recipient;
            EmailAddress = recipient;

            CompanyName = companyName;
            Attention = attention;
        }

        /// <summary>
        /// Initiates Recipient
        /// </summary>
        /// <param name="recipient">Recipient to receive message(s)</param>
        /// <param name="companyName">Company Name - for substitutions</param>
        /// <param name="attention">Attention - for substitutions</param>
        /// <param name="custom1">Custom1 - for substitutions</param>
        /// <param name="custom2">Custom2 - for substitutions</param>
        /// <param name="custom3">Custom3 - for substitutions</param>
        /// <param name="custom4">Custom4 - for substitutions</param>
        /// <param name="custom5">Custom5 - for substitutions</param>
        /// <returns></returns>
        public Recipient(string recipient, string companyName, string attention, string custom1, string custom2, string custom3, string custom4, string custom5)
        {
            MobileNumber = recipient;
            FaxNumber = recipient;
            PhoneNumber = recipient;
            EmailAddress = recipient;

            CompanyName = companyName;
            Attention = attention;

            Custom1 = custom1;
            Custom2 = custom2;
            Custom3 = custom3;
            Custom4 = custom4;
            Custom5 = custom5;
        }

        /// <summary>
        /// Initiates Recipient
        /// </summary>
        /// <param name="recipient">Recipient to receive message(s)</param>
        /// <param name="companyName">Company Name - for substitutions</param>
        /// <param name="attention">Attention - for substitutions</param>
        /// <param name="firstName">First Name - for substitutions</param>
        /// <param name="lastName">Las Name - for substitutions</param>
        /// <param name="custom1">Custom1 - for substitutions</param>
        /// <param name="custom2">Custom2 - for substitutions</param>
        /// <param name="custom3">Custom3 - for substitutions</param>
        /// <param name="custom4">Custom4 - for substitutions</param>
        /// <param name="custom5">Custom5 - for substitutions</param>
        public Recipient(string recipient = null, string companyName = null, string attention = null, string firstName = null, string lastName = null, string custom1 = null, string custom2 = null, string custom3 = null, string custom4 = null, string custom5 = null)
        {
            if (recipient is not null)
            {
                MobileNumber = recipient;
                FaxNumber = recipient;
                PhoneNumber = recipient;
                EmailAddress = recipient;
            }

            if (companyName is not null)
            {
                CompanyName = companyName;
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
        }
    }
}
