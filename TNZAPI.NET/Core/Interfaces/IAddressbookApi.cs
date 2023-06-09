using TNZAPI.NET.Core.Interfaces.Addressbook;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IAddressbookApi
    {
        public IContactApi Contact { get; set; }
        public IContactListApi ContactList { get; set; }
        public IContactGroupApi ContactGroup { get; set; }
        public IContactGroupListApi ContactGroupList { get; set; }

        public IGroupApi Group { get; set; }
        public IGroupListApi GroupList { get; set; }
        public IGroupContactApi GroupContact { get; set; }
        public IGroupContactListApi GroupContactList { get; set; }
    }
}
