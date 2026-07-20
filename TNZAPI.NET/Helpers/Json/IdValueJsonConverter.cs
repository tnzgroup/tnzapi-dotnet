using System.Text.Json;
using System.Text.Json.Serialization;

namespace TNZAPI.NET.Helpers.Json
{
    internal abstract class IdValueJsonConverter<T> : JsonConverter<T> where T : class
    {
        private readonly Func<string, T> _factory;

        protected IdValueJsonConverter(Func<string, T> factory)
        {
            _factory = factory;
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var value = reader.GetString();

            return value is null ? null : _factory(value);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var stringValue = value?.ToString();

            if (stringValue is null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(stringValue);
            }
        }
    }
}