using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using TNZAPI.NET.Api.Reports.SMSReceived.Dto;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces.Reports;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Api.Reports.SMSReceived
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class SMSReceivedApi : ISMSReceivedApi
    {
        private ITNZAuth User = new TNZApiUser();

        private SMSReceivedRequestOptions Options { get; set; } = new SMSReceivedRequestOptions();

        /// <summary>
        /// Get List of SMS Received
        /// </summary>
        /// <returns></returns>
        public SMSReceivedApi()
        {
        }

        /// <summary>
        /// Get List of SMS Received
        /// </summary>
        /// <param name="authToken">Auth Token for TNZAPI</param>
        /// <returns></returns>
        public SMSReceivedApi(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Get List of SMS Received
        /// </summary>
        /// <param name="apiSender">API Username - Email Address</param>
        /// <param name="apiKey">API Key for TNZAPI</param>
        /// <returns></returns>
        public SMSReceivedApi(string apiSender, string apiKey)
        {
            User.Sender = apiSender;
            User.APIKey = apiKey;
        }

        /// <summary>
        /// Get List of SMS Received
        /// </summary>
        /// <param name="apiUser">API User Details</param>
        public SMSReceivedApi(TNZApiUser apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Get List of SMS Received
        /// </summary>
        /// <param name="auth">API User</param>
        public SMSReceivedApi(ITNZAuth auth)
        {
            User = auth;
        }

        private XmlDocument BuildXmlDocument()
        {
            #region XML Sample
            /*
						<root>
							<Sender>application@domain.com</Sender>
							<APIKey>ta8wr7ymd</APIKey>
							<Type>status</Type>
							<APIVersion>1.02</APIVersion>
							<MessageID>ID123456</MessageID>
						</root>      
            */
            #endregion XML Sample

            XmlDocument xmlDoc = new XmlDocument();

            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null); // <?xml version="1.0" encoding="UTF-8"?>
            xmlDoc.AppendChild(docNode);

            XmlNode rootNode = xmlDoc.CreateElement("SMSReceivedRequest");
            xmlDoc.AppendChild(rootNode);

            if (User.AuthToken.Equals(""))
            {
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Sender", User.Sender));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "APIKey", User.APIKey));

                if (Options.DateFrom > DateTime.MinValue && Options.DateTo < DateTime.MaxValue)
                {
                    rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "DateFrom", Options.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss")));
                    rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "DateTo", Options.DateTo.ToString("yyyy-MM-ddTHH:mm:ss")));
                }
                else
                {
                    rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "TimePeriod", Options.TimePeriod.ToString()));
                }
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "RecordsPerPage", Options.RecordsPerPage.ToString()));
                rootNode.AppendChild(XMLHelpers.addChildNode(xmlDoc, "Page", Options.Page.ToString()));
            }

            return xmlDoc;
        }

        private string BuildAPIURL()
        {
            var requestUri = new StringBuilder();

            requestUri.Append($"{TNZApiConfig.Domain}/api/v{TNZApiConfig.Version}/get/sms/received");

            if (User.AuthToken.Equals("") == false)
            {
                requestUri.Append("?");

                if (Options.DateFrom > DateTime.MinValue && Options.DateTo < DateTime.MaxValue)
                {
                    requestUri.Append($"dateFrom={Options.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss")}&dateTo={Options.DateTo.ToString("yyyy-MM-ddTHH:mm:ss")}");
                }
                else
                {
                    requestUri.Append($"timePeriod={Options.TimePeriod}");
                }

                requestUri.Append($"&recordsPerPage={Options.RecordsPerPage}&page={Options.Page}");
            }

            return requestUri.ToString();
        }

        // Synchronous function for backward compatibility
        private SMSReceivedApiResult SendXML()
        {
            try
            {
                return Task.Run(() => HttpRequest.GetXMLAsync<SMSReceivedApiResult>(
                    new GetHttpRequest
                    (
                        BuildAPIURL(),
                        User,
                        BuildXmlDocument()
                    ))).Result;
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>(e.Message);
            }
        }

        // PATCH XML to TNZ REST API
        private async Task<SMSReceivedApiResult> SendXMLAsync()
        {
            try
            {
                return
                    await HttpRequest.GetXMLAsync<SMSReceivedApiResult>(
                        new GetHttpRequest
                        (
                            BuildAPIURL(),
                            User,
                            BuildXmlDocument()
                        ));
            }
            catch (Exception e)
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>(e.Message);
            }
        }

        #region Set API User
        /// <summary>
        /// Sets the API User
        /// </summary>
        /// <param name="apiUser">IAuth</param>
        public void SetAPIUser(ITNZAuth apiUser)
        {
            User = apiUser;
        }

        /// <summary>
        /// Sets the Authorization Token
        /// </summary>
        /// <param name="authToken">Authorization Token</param>
        public void SetAuthToken(string authToken)
        {
            User.AuthToken = authToken;
        }

        /// <summary>
        /// Sets the API Sender
        /// </summary>
        /// <param name="apiSender">Sender</param>
        public void SetAPISender(string apiSender)
        {
            User.Sender = apiSender;
        }

        /// <summary>
        /// Sets the API Key
        /// </summary>
        /// <param name="apiKey">API Key</param>
        public void SetAPIKey(string apiKey)
        {
            User.APIKey = apiKey;
        }
        #endregion

        #region List
        private SMSReceivedApiResult List()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<SMSReceivedApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<SMSReceivedApiResult>("Empty API key: Please specify APIKey");
                }
            }

            if (Options is null)
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>("Empty Options: Please set options");
            }
            if (Options.TimePeriod <= 0)
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>("TimePeriod must be greater then 0");
            }
            if (Options.DateFrom > DateTime.MinValue && Options.DateTo < DateTime.MaxValue)
            {
                if (Options.DateFrom >= Options.DateTo)
                {
                    return ResultHelper.RespondError<SMSReceivedApiResult>("DateTo must be greater than DateFrom");
                }
            }

            return SendXML();
        }

        /// <summary>
        /// List SMS Received
        /// </summary>
        /// <param name="options">SMSReceivedOptions</param>
        /// <returns>SMSReceivedResult</returns>
        [ComVisible(false)]
        public SMSReceivedApiResult List(SMSReceivedRequestOptions options)
        {
            Options = Mapper.Update(new SMSReceivedRequestOptions(), options);

            return List();
        }

        /// <summary>
        /// List SMS Received
        /// </summary>
        /// <param name="timePeriod">Time period in minutes</param>
        /// <param name="dateFrom">From Date/Time</param>
        /// <param name="dateTo">To Date/Time</param>
        /// <param name="recordsPerPage">Number of records per page</param>
        /// <param name="page">Current page number</param>
        /// <returns>SMSReceivedResult</returns>
        [ComVisible(false)]
        public SMSReceivedApiResult List(
            int? timePeriod = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            int? recordsPerPage = null,
            int? page = null
        )
        {
            var options = new SMSReceivedRequestOptions();

            if (timePeriod is not null)
            {
                options.TimePeriod = (int)timePeriod;
            }
            if (dateFrom is not null)
            {
                options.DateFrom = (DateTime)dateFrom;
            }
            if (dateTo is not null)
            {
                options.DateTo = (DateTime)dateTo;
            }
            if (recordsPerPage is not null)
            {
                options.RecordsPerPage = (int)recordsPerPage;
            }
            if (page is not null)
            {
                options.Page = (int)page;
            }

            return List(options);
        }

        #endregion List

        #region ListAsync
        /// <summary>
        /// List SMS Received (async)
        /// </summary>
        /// <returns></returns>
        [ComVisible(false)]
        private async Task<SMSReceivedApiResult> ListAsync()
        {
            if (User.AuthToken.Equals(""))
            {
                if (User.Sender.Equals(""))
                {
                    return ResultHelper.RespondError<SMSReceivedApiResult>("Empty sender: Please specify Sender");
                }
                if (User.APIKey.Equals(""))
                {
                    return ResultHelper.RespondError<SMSReceivedApiResult>("Empty API key: Please specify APIKey");
                }
            }

            if (Options is null)
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>("Empty Options: Please set options");
            }

            if (Options.TimePeriod <= 0)
            {
                return ResultHelper.RespondError<SMSReceivedApiResult>("TimePeriod must be greater then 0");
            }

            if (Options.DateFrom > DateTime.MinValue && Options.DateTo < DateTime.MaxValue)
            {
                if (Options.DateFrom >= Options.DateTo)
                {
                    return ResultHelper.RespondError<SMSReceivedApiResult>("DateTo must be greater than DateFrom");
                }
            }

            return await SendXMLAsync();
        }

        /// <summary>
        /// List SMS Receivedt (async)
        /// </summary>
        /// <param name="options">SMSReceivedOptions</param>
        /// <returns>Task<SMSReceivedResult></returns>
        [ComVisible(false)]
        public async Task<SMSReceivedApiResult> ListAsync(SMSReceivedRequestOptions options)
        {
            Options = Mapper.Update(new SMSReceivedRequestOptions(), options);

            return await ListAsync();
        }

        /// <summary>
        /// List SMS Received (async)
        /// </summary>
        /// <param name="timePeriod">Time period in minutes</param>
        /// <param name="dateFrom">From Date/Time</param>
        /// <param name="dateTo">To Date/Time</param>
        /// <param name="recordsPerPage">Number of records per page</param>
        /// <param name="page">Current page number</param>
        /// <returns>Task<SMSReceivedResult></returns>
        [ComVisible(false)]
        public async Task<SMSReceivedApiResult> ListAsync(
            int? timePeriod = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            int? recordsPerPage = null,
            int? page = null
        )
        {
            var options = new SMSReceivedRequestOptions();

            if (timePeriod is not null)
            {
                options.TimePeriod = (int)timePeriod;
            }
            if (dateFrom is not null)
            {
                options.DateFrom = (DateTime)dateFrom;
            }
            if (dateTo is not null)
            {
                options.DateTo = (DateTime)dateTo;
            }
            if (recordsPerPage is not null)
            {
                options.RecordsPerPage = (int)recordsPerPage;
            }
            if (page is not null)
            {
                options.Page = (int)page;
            }

            return await ListAsync(options);
        }

        #endregion ListAsync
    }
}
