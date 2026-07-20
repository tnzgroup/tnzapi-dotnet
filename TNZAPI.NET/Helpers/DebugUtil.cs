using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Helpers
{
    public static class DebugUtil
    {
        public static string Dump<T>(this T obj, bool display=true)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                // Same converter set/order as HttpRequest's JsonOptions, so a dumped SDK result
                // renders wire-shaped values (e.g. RecipientChannelType, timestamps) instead of
                // raw CLR defaults.
                Converters = { new FlexibleRecipientChannelTypeJsonConverter(), new JsonStringEnumConverter(), new FlexibleDateTimeJsonConverter() }
            };

            var jsonString = JsonSerializer.Serialize(obj, jsonOptions);

            if (display)
            {
                Debug.WriteLine(jsonString);
            }

            return jsonString;
        }
    }

}
