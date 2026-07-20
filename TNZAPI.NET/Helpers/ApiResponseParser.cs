using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using TNZAPI.NET.Core;
using TNZAPI.NET.Core.Interfaces;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Helpers
{
    internal static class ApiResponseParser
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            // FlexibleRecipientChannelTypeJsonConverter must come before JsonStringEnumConverter —
            // JsonStringEnumConverter is a JsonConverterFactory whose CanConvert matches ANY enum, so
            // if it's registered first it swallows RecipientChannelType before the flexible converter
            // ever gets a chance (System.Text.Json picks the first matching converter in list order).
            Converters = { new FlexibleRecipientChannelTypeJsonConverter(), new JsonStringEnumConverter(), new FlexibleDateTimeJsonConverter() }
        };

        internal static async Task<T> ParseAsync<T>(HttpResponseMessage response) where T : class, IApiResult, new()
        {
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (string.IsNullOrWhiteSpace(body))
                {
                    return new T
                    {
                        Result = Enums.ResultCode.Failed,
                        ErrorMessage = new List<string> { $"Received a successful HTTP status ({(int)response.StatusCode}) but the response body was empty." }
                    };
                }

                try
                {
                    var result = JsonSerializer.Deserialize<T>(body, JsonOptions) ?? new T();
                    result.Result = Enums.ResultCode.Success;
                    result.ErrorMessage ??= new List<string>();

                    return result;
                }
                catch (JsonException ex)
                {
                    return new T
                    {
                        Result = Enums.ResultCode.Failed,
                        ErrorMessage = new List<string> { $"Received a successful HTTP status ({(int)response.StatusCode}) but the response body could not be parsed: {ex.Message}" }
                    };
                }
            }

            var errorResult = new T
            {
                Result = MapStatusCodeToResultCode(response.StatusCode),
                ErrorMessage = ExtractErrorMessages(body)
            };

            return errorResult;
        }

        private static Enums.ResultCode MapStatusCodeToResultCode(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.Unauthorized => Enums.ResultCode.Unauthorized,
                HttpStatusCode.NotFound => Enums.ResultCode.RecordNotFound,
                _ => Enums.ResultCode.Failed
            };
        }

        private static List<string> ExtractErrorMessages(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return new List<string>();
            }

            try
            {
                using var doc = JsonDocument.Parse(body);

                if (!doc.RootElement.TryGetProperty("ErrorMessage", out var errorMessageElement))
                {
                    return new List<string>();
                }

                if (errorMessageElement.ValueKind == JsonValueKind.Array)
                {
                    return errorMessageElement.EnumerateArray()
                        .Select(e => e.GetString() ?? string.Empty)
                        .Where(s => s.Length > 0)
                        .ToList();
                }

                if (errorMessageElement.ValueKind == JsonValueKind.String)
                {
                    var value = errorMessageElement.GetString();

                    return string.IsNullOrEmpty(value) ? new List<string>() : new List<string> { value! };
                }

                return new List<string>();
            }
            catch (JsonException)
            {
                return new List<string>();
            }
        }
    }
}