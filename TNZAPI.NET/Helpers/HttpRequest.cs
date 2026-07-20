using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Helpers
{
    internal static class HttpRequest
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            // FlexibleRecipientChannelTypeJsonConverter must come before JsonStringEnumConverter —
            // JsonStringEnumConverter is a JsonConverterFactory whose CanConvert matches ANY enum, so
            // if it's registered first it swallows RecipientChannelType before the flexible converter
            // ever gets a chance (System.Text.Json picks the first matching converter in list order).
            Converters = { new FlexibleRecipientChannelTypeJsonConverter(), new JsonStringEnumConverter(), new FlexibleDateTimeJsonConverter() },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        // Test seam: production code leaves this at its default; tests swap it for a fake handler.
        internal static HttpMessageHandler MessageHandler { get; set; } = new HttpClientHandler();

        internal static Task<T> GetAsync<T>(string uri, ITNZAuth user) where T : class, IApiResult, new()
        {
            return SendAsync<T>(HttpMethod.Get, uri, user, null);
        }

        internal static Task<T> PostAsync<T>(string uri, ITNZAuth user, object body) where T : class, IApiResult, new()
        {
            return SendAsync<T>(HttpMethod.Post, uri, user, body);
        }

        internal static Task<T> PatchAsync<T>(string uri, ITNZAuth user, object body) where T : class, IApiResult, new()
        {
            return SendAsync<T>(new HttpMethod("PATCH"), uri, user, body);
        }

        internal static Task<T> DeleteAsync<T>(string uri, ITNZAuth user) where T : class, IApiResult, new()
        {
            return SendAsync<T>(HttpMethod.Delete, uri, user, null);
        }

        private static async Task<T> SendAsync<T>(HttpMethod method, string uri, ITNZAuth user, object? body) where T : class, IApiResult, new()
        {
            if (!uri.StartsWith("https://", StringComparison.OrdinalIgnoreCase) && !string.Equals(Environment.GetEnvironmentVariable("TNZ_ALLOW_INSECURE_HTTP"), "true", StringComparison.OrdinalIgnoreCase))
            {
                return ResultHelper.RespondError<T>("TNZ API URL must use HTTPS — refusing to send the Authorization bearer token over plain HTTP. Set TNZ_ALLOW_INSECURE_HTTP=true to override for local development.");
            }

            using var client = new HttpClient(MessageHandler, disposeHandler: false);
            using var message = new HttpRequestMessage(method, uri);

            message.Headers.Add("User-Agent", TNZApiConfig.UserAgent);
            message.Headers.Add("Accept", "application/json");
            message.Headers.Add("Authorization", $"Bearer {user.AuthToken}");

            if (body is not null)
            {
                var json = JsonSerializer.Serialize(body, JsonOptions);
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            using var response = await client.SendAsync(message);

            return await ApiResponseParser.ParseAsync<T>(response);
        }
    }
}