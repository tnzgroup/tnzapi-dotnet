using TNZAPI.NET.Api.Actions;
using TNZAPI.NET.Api.Addressbook;
using TNZAPI.NET.Api.Configuration;
using TNZAPI.NET.Api.Messaging;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Core
{
    public class TNZApiClient : ITNZApiClient
    {
        private ITNZAuth User { get; set; }

        public IMessagingApi Messaging { get; set; }
        public IActionsApi Actions { get; set; }
        public IAddressbookApi Addressbook { get; set; }
        public IConfigurationApi Configuration { get; set; }

        /// <summary>
        /// Initiates TNZApiClient
        /// </summary>
        public TNZApiClient()
        {
            User = new TNZApiUser();

            (Messaging, Actions, Addressbook, Configuration) = BuildFacades(User);
        }

        /// <summary>
        /// Initiates TNZApiClient
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        public TNZApiClient(string authToken)
        {
            User = new TNZApiUser
            {
                AuthToken = authToken
            };

            (Messaging, Actions, Addressbook, Configuration) = BuildFacades(User);
        }

        /// <summary>
        /// Initiates TNZApiClient
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public TNZApiClient(TNZApiUser apiUser)
        {
            User = apiUser;

            (Messaging, Actions, Addressbook, Configuration) = BuildFacades(User);
        }

        /// <summary>
        /// Initiates TNZApiClient
        /// </summary>
        /// <param name="auth">IAuth</param>
        public TNZApiClient(ITNZAuth auth)
        {
            User = auth;

            (Messaging, Actions, Addressbook, Configuration) = BuildFacades(User);
        }

        private static (IMessagingApi, IActionsApi, IAddressbookApi, IConfigurationApi) BuildFacades(ITNZAuth user)
        {
            return (new MessagingApi(user), new ActionsApi(user), new AddressbookApi(user), new ConfigurationApi(user));
        }
    }
}