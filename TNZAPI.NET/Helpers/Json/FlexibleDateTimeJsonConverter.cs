using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TNZAPI.NET.Helpers.Json
{
    internal sealed class FlexibleDateTimeJsonConverter : JsonConverter<DateTime>
    {
        private const string TNZTimestampFormat = "yyyy-MM-dd HH:mm:ss";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed))
            {
                return parsed;
            }

            if (DateTime.TryParseExact(value, TNZTimestampFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
            {
                return parsed;
            }

            throw new JsonException($"Unable to parse '{value}' as a DateTime.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var formatted = value.ToString("O", CultureInfo.InvariantCulture);

            if (value.Ticks % TimeSpan.TicksPerSecond == 0)
            {
                var fractionStart = formatted.IndexOf('.');

                if (fractionStart >= 0)
                {
                    var fractionEnd = fractionStart + 1;

                    while (fractionEnd < formatted.Length && char.IsDigit(formatted[fractionEnd]))
                    {
                        fractionEnd++;
                    }

                    formatted = formatted.Remove(fractionStart, fractionEnd - fractionStart);
                }
            }

            writer.WriteStringValue(formatted);
        }
    }
}