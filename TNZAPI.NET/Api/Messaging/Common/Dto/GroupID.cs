using System.Text.Json.Serialization;
using TNZAPI.NET.Helpers.Json;

namespace TNZAPI.NET.Api.Messaging.Common.Dto
{
    [JsonConverter(typeof(GroupIDJsonConverter))]
    public record GroupID
    {
        public string? Value { get; set; }

        public GroupID(string value)
        {
            Value = value;
        }

        public GroupID(object obj)
        {
            if (obj is string s)
            {
                Value = s;
                return;
            }
            if (obj is GroupID groupID)
            {
                Value = groupID;
                return;
            }

            throw new Exception($"Unsupported type - {obj}");
        }

        public GroupID()
        {
        }

        public static implicit operator string?(GroupID groupID) => groupID?.Value;

        public override string? ToString()
        {
            if (Value is null)
            {
                return null;
            }

            return $"{Value}";
        }
    }

    internal sealed class GroupIDJsonConverter : IdValueJsonConverter<GroupID>
    {
        public GroupIDJsonConverter() : base(v => new GroupID(v))
        {
        }
    }
}