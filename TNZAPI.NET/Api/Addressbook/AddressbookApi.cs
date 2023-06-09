using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Contact.Group;
using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Api.Addressbook.Group.Contact;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Addressbook;

namespace TNZAPI.NET.Api.Addressbook
{
    public class AddressbookApi : IAddressbookApi
    {
        public IContactApi Contact { get; set; }
        public IContactListApi ContactList { get; set; }
        public IContactGroupApi ContactGroup { get; set; }
        public IContactGroupListApi ContactGroupList { get; set; }

        public IGroupApi Group { get; set; }
        public IGroupListApi GroupList { get; set; }
        public IGroupContactApi GroupContact { get; set; }
        public IGroupContactListApi GroupContactList { get; set; }

        /// <summary>
        /// Initiates AddressbookApi to send through TNZAPI
        /// </summary>
        public AddressbookApi(ITNZAuth user)
        {
            Contact = new ContactApi(user);
            ContactList = new ContactListApi(user);
            ContactGroup = new ContactGroupApi(user);
            ContactGroupList = new ContactGroupListApi(user);

            Group = new GroupApi(user);
            GroupList = new GroupListApi(user);
            GroupContact = new GroupContactApi(user);
            GroupContactList = new GroupContactListApi(user);
        }
    }
}
