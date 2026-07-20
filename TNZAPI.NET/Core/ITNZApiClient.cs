using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Core
{
    public interface ITNZApiClient
    {
        IMessagingApi Messaging { get; set; }
        IActionsApi Actions { get; set; }
        IAddressbookApi Addressbook { get; set; }
        IConfigurationApi Configuration { get; set; }
    }
}