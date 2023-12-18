using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Core
{
	public interface ITNZApiClient
	{
		IActionsApi Actions { get; set; }
		IAddressbookApi Addressbook { get; set; }
		IMessagingApi Messaging { get; set; }
		IReportsApi Reports { get; set; }
	}
}