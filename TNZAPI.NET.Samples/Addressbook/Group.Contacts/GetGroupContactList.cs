﻿using TNZAPI.NET.Api.Addressbook.Group.Contact.Dto;
using TNZAPI.NET.Api.Addressbook.Group.Dto;
using TNZAPI.NET.Api.Messaging.Common;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Samples.Addressbook.Group.Contacts
{
    public class GetGroupContactList
    {
        private readonly ITNZAuth apiUser;

        public GroupModel Group { get; set; }

        public GetGroupContactList(ITNZAuth apiUser)
        {
            this.apiUser = apiUser;

            Group = new GroupModel();
        }

        public GetGroupContactList(ITNZAuth apiUser, GroupModel group)
        {
            this.apiUser = apiUser;

            Group = group;
        }

        public GroupContactListApiResult Basic()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.GroupContactList.List(
                Group,                  // Group
                recordsPerPage: 10,     // Number of records for this request
                page: 1                 // Current location
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Group Contact list details");
                Console.WriteLine($"    -> GroupCode: {response.Group.GroupCode}");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                foreach (var contact in response.Contacts)
                {
                    Console.WriteLine($"Contact details for ContactID={contact.ID}");
                    Console.WriteLine($"    -> Owner: '{contact.Owner}'");
                    Console.WriteLine($"    -> Created: '{contact.Created}'");
                    Console.WriteLine($"    -> Updated: '{contact.Updated}'");
                    Console.WriteLine($"    -> Attention: '{contact.Attention}'");
                    Console.WriteLine($"    -> Company: '{contact.Company}'");
                    Console.WriteLine($"    -> RecipDepartment: '{contact.CompanyDepartment}'");
                    Console.WriteLine($"    -> FirstName: '{contact.FirstName}'");
                    Console.WriteLine($"    -> LastName: '{contact.LastName}'");
                    Console.WriteLine($"    -> Position: '{contact.Position}'");
                    Console.WriteLine($"    -> StreetAddress: '{contact.StreetAddress}'");
                    Console.WriteLine($"    -> Suburb: '{contact.Suburb}'");
                    Console.WriteLine($"    -> City: '{contact.City}'");
                    Console.WriteLine($"    -> State: '{contact.State}'");
                    Console.WriteLine($"    -> Country: '{contact.Country}'");
                    Console.WriteLine($"    -> Postcode: '{contact.Postcode}'");
                    Console.WriteLine($"    -> MainPhone: '{contact.MainPhone}'");
                    Console.WriteLine($"    -> AltPhone1: '{contact.AltPhone1}'");
                    Console.WriteLine($"    -> AltPhone2: '{contact.AltPhone2}'");
                    Console.WriteLine($"    -> DirectPhone: '{contact.DirectPhone}'");
                    Console.WriteLine($"    -> MobilePhone: '{contact.MobilePhone}'");
                    Console.WriteLine($"    -> FaxNumber: '{contact.FaxNumber}'");
                    Console.WriteLine($"    -> EmailAddress: '{contact.EmailAddress}'");
                    Console.WriteLine($"    -> WebAddress: '{contact.WebAddress}'");
                    Console.WriteLine($"    -> Custom1: '{contact.Custom1}'");
                    Console.WriteLine($"    -> Custom2: '{contact.Custom2}'");
                    Console.WriteLine($"    -> Custom3: '{contact.Custom3}'");
                    Console.WriteLine($"    -> Custom4: '{contact.Custom4}'");
                    Console.WriteLine($"-------------------------");
                }
            }
            else
            {
                Console.WriteLine($"Could not find any contact list");
                if (response.ErrorMessage is not null)
                {
                    Console.WriteLine($"Error={response.ErrorMessage}");
                }
            }

            return response;
        }

        public GroupContactListApiResult Simple() => Basic();    // Same as Basic

        public GroupContactListApiResult Builder() => Basic();   // Same as Basic

        public GroupContactListApiResult Advanced()
        {
            var client = new TNZApiClient(apiUser);

            var response = client.Addressbook.GroupContactList.List(
                Group,                      // Group
                new GroupContactListRequestOptions()
                {
                    RecordsPerPage = 10,    // Number of records for this request
                    Page = 1                // Current location

                }
            );

            if (response.Result == Enums.ResultCode.Success)
            {
                Console.WriteLine($"Group Contact list details");
                Console.WriteLine($"    -> GroupCode: {response.Group.GroupCode}");
                Console.WriteLine($"    -> Total Records: {response.TotalRecords}");
                Console.WriteLine($"    -> Records Per Page: {response.RecordsPerPage}");
                Console.WriteLine($"    -> Page Count: {response.PageCount}");
                Console.WriteLine($"    -> Page: {response.Page}");

                foreach (var contact in response.Contacts)
                {
                    Console.WriteLine($"Contact details for ContactID={contact.ID}");
                    Console.WriteLine($"    -> Owner: '{contact.Owner}'");
                    Console.WriteLine($"    -> Created: '{contact.Created}'");
                    Console.WriteLine($"    -> Updated: '{contact.Updated}'");
                    Console.WriteLine($"    -> Attention: '{contact.Attention}'");
                    Console.WriteLine($"    -> Company: '{contact.Company}'");
                    Console.WriteLine($"    -> RecipDepartment: '{contact.CompanyDepartment}'");
                    Console.WriteLine($"    -> FirstName: '{contact.FirstName}'");
                    Console.WriteLine($"    -> LastName: '{contact.LastName}'");
                    Console.WriteLine($"    -> Position: '{contact.Position}'");
                    Console.WriteLine($"    -> StreetAddress: '{contact.StreetAddress}'");
                    Console.WriteLine($"    -> Suburb: '{contact.Suburb}'");
                    Console.WriteLine($"    -> City: '{contact.City}'");
                    Console.WriteLine($"    -> State: '{contact.State}'");
                    Console.WriteLine($"    -> Country: '{contact.Country}'");
                    Console.WriteLine($"    -> Postcode: '{contact.Postcode}'");
                    Console.WriteLine($"    -> MainPhone: '{contact.MainPhone}'");
                    Console.WriteLine($"    -> AltPhone1: '{contact.AltPhone1}'");
                    Console.WriteLine($"    -> AltPhone2: '{contact.AltPhone2}'");
                    Console.WriteLine($"    -> DirectPhone: '{contact.DirectPhone}'");
                    Console.WriteLine($"    -> MobilePhone: '{contact.MobilePhone}'");
                    Console.WriteLine($"    -> FaxNumber: '{contact.FaxNumber}'");
                    Console.WriteLine($"    -> EmailAddress: '{contact.EmailAddress}'");
                    Console.WriteLine($"    -> WebAddress: '{contact.WebAddress}'");
                    Console.WriteLine($"    -> Custom1: '{contact.Custom1}'");
                    Console.WriteLine($"    -> Custom2: '{contact.Custom2}'");
                    Console.WriteLine($"    -> Custom3: '{contact.Custom3}'");
                    Console.WriteLine($"    -> Custom4: '{contact.Custom4}'");
                    Console.WriteLine($"-------------------------");
                }
            }
            else
            {
                Console.WriteLine($"Could not find any contact list");
                if (response.ErrorMessage is not null)
                {
                    Console.WriteLine($"Error={response.ErrorMessage}");
                }
            }

            return response;
        }
    }
}
