﻿using TNZAPI.NET.Api.Addressbook.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Contact.Group.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Contact.Groups
{
    public class AddContactGroup
    {
        private readonly ITNZAuth apiUser;

        public ContactModel Contact;

        public GroupModel Group;

        public AddContactGroup(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            Contact = new ContactModel();

            Group = new GroupModel();
        }

        public AddContactGroup(ITNZAuth apiUser, ContactModel contact, GroupModel group)
        {
            this.apiUser = apiUser;

            Contact = contact;

            Group = group;
        }

        public ContactGroupApiResult? Basic()
        {
            var request = new TNZApiClient(apiUser);

            var response = request.Addressbook.ContactGroup.Add(Contact, Group);

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Contact details for ContactID={response.Contact.ID}");
                Console.WriteLine($"    -> Owner: '{response.Contact.Owner}'");
                Console.WriteLine($"    -> Created: '{response.Contact.Created}'");
                Console.WriteLine($"    -> Updated: '{response.Contact.Updated}'");
                Console.WriteLine($"    -> Attention: '{response.Contact.Attention}'");
                Console.WriteLine($"    -> Company: '{response.Contact.Company}'");
                Console.WriteLine($"    -> RecipDepartment: '{response.Contact.CompanyDepartment}'");
                Console.WriteLine($"    -> FirstName: '{response.Contact.FirstName}'");
                Console.WriteLine($"    -> LastName: '{response.Contact.LastName}'");
                Console.WriteLine($"    -> Position: '{response.Contact.Position}'");
                Console.WriteLine($"    -> StreetAddress: '{response.Contact.StreetAddress}'");
                Console.WriteLine($"    -> Suburb: '{response.Contact.Suburb}'");
                Console.WriteLine($"    -> City: '{response.Contact.City}'");
                Console.WriteLine($"    -> State: '{response.Contact.State}'");
                Console.WriteLine($"    -> Country: '{response.Contact.Country}'");
                Console.WriteLine($"    -> Postcode: '{response.Contact.Postcode}'");
                Console.WriteLine($"    -> MainPhone: '{response.Contact.MainPhone}'");
                Console.WriteLine($"    -> AltPhone1: '{response.Contact.AltPhone1}'");
                Console.WriteLine($"    -> AltPhone2: '{response.Contact.AltPhone2}'");
                Console.WriteLine($"    -> DirectPhone: '{response.Contact.DirectPhone}'");
                Console.WriteLine($"    -> MobilePhone: '{response.Contact.MobilePhone}'");
                Console.WriteLine($"    -> FaxNumber: '{response.Contact.FaxNumber}'");
                Console.WriteLine($"    -> EmailAddress: '{response.Contact.EmailAddress}'");
                Console.WriteLine($"    -> WebAddress: '{response.Contact.WebAddress}'");
                Console.WriteLine($"    -> Custom1: '{response.Contact.Custom1}'");
                Console.WriteLine($"    -> Custom2: '{response.Contact.Custom2}'");
                Console.WriteLine($"    -> Custom3: '{response.Contact.Custom3}'");
                Console.WriteLine($"    -> Custom4: '{response.Contact.Custom4}'");
                Console.WriteLine($"-------------------------");

                if (response.Group is not null)
                {
                    Console.WriteLine($"Group details for GroupCode={response.Group.GroupCode}");
                    Console.WriteLine($"    -> GroupCode: '{response.Group.GroupCode}'");
                    Console.WriteLine($"    -> GroupName: '{response.Group.GroupName}'");
                    Console.WriteLine($"    -> SubAccount: '{response.Group.SubAccount}'");
                    Console.WriteLine($"    -> Department: '{response.Group.Department}'");
                    Console.WriteLine($"    -> ViewEditBy: '{response.Group.ViewEditBy}'");
                    Console.WriteLine($"    -> Owner: '{response.Group.Owner}'");
                    Console.WriteLine($"-------------------------");
                }

                return response;
            }
            else
            {
                Console.WriteLine($"Could not find any contact with ID={Contact.ID}");
            }

            return null;
        }
    }
}