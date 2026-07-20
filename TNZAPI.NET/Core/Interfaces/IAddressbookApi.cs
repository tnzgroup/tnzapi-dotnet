using TNZAPI.NET.Core.Interfaces.Addressbook;

namespace TNZAPI.NET.Core.Interfaces
{
    public interface IAddressbookApi
    {
        IContactApi Contact { get; set; }
        IGroupApi Group { get; set; }
    }
}