using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Core.Interfaces.Addressbook;

namespace TNZAPI.NET.Api.Addressbook
{
    public class AddressbookApi : IAddressbookApi
    {
        public IContactApi Contact { get; set; }
        public IGroupApi Group { get; set; }

        public AddressbookApi(ITNZAuth auth)
        {
            Contact = new ContactApi(auth);
            Group = new GroupApi(auth);
        }
    }
}