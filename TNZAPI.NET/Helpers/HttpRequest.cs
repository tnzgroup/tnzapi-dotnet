using System.Net;
using System.Text;
using System.Xml;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Helpers
{
    internal record GetHttpRequest(string Uri, ITNZAuth APIUser, XmlDocument XmlDoc);
    internal record PostHttpRequest(string Uri, ITNZAuth APIUser, XmlDocument XmlDoc);
    internal record PatchHttpRequest(string Uri, ITNZAuth APIUser, XmlDocument XmlDoc);
    internal record DeleteHttpRequest(string Uri, ITNZAuth APIUser, XmlDocument XmlDoc);

    internal record SendHttpRequest(HttpMethod HttpMethod, string Uri, ITNZAuth APIUser, string Content);

    internal class HttpRequest
    {
        //
        // Using Generics
        //

        private static async Task<T> ProcessHttpRequest<T>(SendHttpRequest request) where T : class
        {
            //
            // Send message using XML Rest API
            //

            using (var client = new HttpClient())
            {
                var message = new HttpRequestMessage(request.HttpMethod, request.Uri);

                message.Headers.Add("User-Agent", TNZApiConfig.UserAgent);
                message.Headers.Add("Accept", "text/xml; encoding=utf-8");

                if (request.APIUser.AuthToken != "")
                {
                    message.Headers.Add("Authorization", $"Basic {request.APIUser.AuthToken}");
                }

                message.Content = new StringContent(request.Content, Encoding.UTF8, "text/xml");

                var result = new StringBuilder();

                try
                {
                    using (var response = await client.SendAsync(message))
                    {
                        if (response != null)
                        {
                            result.Append(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
                catch (WebException wex)
                {
                    if (wex.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)wex.Response)
                        {
                            using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                            {
                                result.Append(reader.ReadToEnd());
                            }
                        }
                    }
                }

                // XMLParser.ParseXML<T> scans assembly and find method based on matching return type
                //return (T)Convert.ChangeType(XMLParser.ParseXML<T>(result.ToString()), typeof(T));                

                return XMLHelpers.Deserialize<T>(result.ToString());
            }
        }

        internal static async Task<T> GetXMLAsync<T>(GetHttpRequest request) where T : class
        {
            // Backward compatibly, historically TNZ API did not support PATCH method
            if (request.APIUser.AuthToken == "")
            {
                return await PostXMLAsync<T>(new PostHttpRequest(request.Uri, request.APIUser, request.XmlDoc));
            }

            //
            // Send message to TNZ REST API
            //

            return await ProcessHttpRequest<T>(new SendHttpRequest(HttpMethod.Get, request.Uri, request.APIUser, ""));
        }

        internal static async Task<T> PostXMLAsync<T>(PostHttpRequest request) where T : class
        {
            //
            // Send message to TNZ REST API
            //

            return await ProcessHttpRequest<T>(new SendHttpRequest(HttpMethod.Post, request.Uri, request.APIUser, request.XmlDoc.OuterXml));
        }

        internal static async Task<T> PatchXMLAsync<T>(PatchHttpRequest request) where T : class
        {
            // Backward compatibly, historically TNZ API did not support PATCH method
            if (request.APIUser.AuthToken == "")
            {
                return await PostXMLAsync<T>(new PostHttpRequest(request.Uri, request.APIUser, request.XmlDoc));
            }

            //
            // Send message to TNZ REST API
            //

            return await ProcessHttpRequest<T>(new SendHttpRequest(HttpMethod.Patch, request.Uri, request.APIUser, request.XmlDoc.OuterXml));
        }

        internal static async Task<T> DeleteXMLAsync<T>(DeleteHttpRequest request) where T : class
        {
            // Backward compatibly, historically TNZ API did not support PATCH method
            if (request.APIUser.AuthToken == "")
            {
                return await DeleteXMLAsync<T>(new DeleteHttpRequest(request.Uri, request.APIUser, request.XmlDoc));
            }

            //
            // Send message to TNZ REST API
            //

            return await ProcessHttpRequest<T>(new SendHttpRequest(HttpMethod.Delete, request.Uri, request.APIUser, request.XmlDoc.OuterXml));
        }
    }
}
