﻿using TNZAPI.NET.Api.Actions;
using TNZAPI.NET.Api.Addressbook;
using TNZAPI.NET.Api.Messaging;
using TNZAPI.NET.Api.Reports;
using TNZAPI.NET.Core.Interfaces;

namespace TNZAPI.NET.Core
{
	public class TNZApiClient : ITNZApiClient
	{
		private ITNZAuth User { get; set; }

		public IAddressbookApi Addressbook { get; set; }
		public IActionsApi Actions { get; set; }
		public IReportsApi Reports { get; set; }
		public IMessagingApi Messaging { get; set; }

		/// <summary>
		/// Initiates Email message to send through TNZAPI
		/// </summary>
		public TNZApiClient()
		{
		}

		/// <summary>
		/// Initiates Email message to send through TNZAPI
		/// </summary>
		/// <param name="authToken">Auth Token for TNZAPI</param>
		public TNZApiClient(string authToken)
		{
			User.AuthToken = authToken;

			Initialise();
		}

		/// <summary>
		/// Initiates Email message to send through TNZAPI
		/// </summary>
		/// <param name="sender">API Username - Email Address</param>
		/// <param name="apiKey">API Key for TNZAPI</param>
		public TNZApiClient(string sender, string apiKey)
		{
			User.Sender = sender;
			User.APIKey = apiKey;

			Initialise();
		}

		/// <summary>
		/// Initiates Email message to send through TNZAPI
		/// </summary>
		/// <param name="apiUser">API User Details</param>
		public TNZApiClient(TNZApiUser apiUser)
		{
			User = apiUser;

			Initialise();
		}

		/// <summary>
		/// Initiates Email message to send through TNZAPI
		/// </summary>
		public TNZApiClient(ITNZAuth auth)
		{
			User = auth;

			Initialise();
		}

		private void Initialise()
		{
			Addressbook = new AddressbookApi(User);
			Actions = new ActionsApi(User);
			Reports = new ReportsApi(User);
			Messaging = new MessagingApi(User);
		}
	}
}
