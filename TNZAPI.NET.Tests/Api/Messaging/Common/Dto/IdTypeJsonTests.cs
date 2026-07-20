using System.Text.Json;
using TNZAPI.NET.Api.Messaging.Common.Dto;

namespace TNZAPI.NET.Tests.Api.Messaging.Common.Dto;

public class IdTypeJsonTests
{
    [Fact]
    public void MessageID_SerializesAsBareString()
    {
        var id = new MessageID("abc-123");

        var json = JsonSerializer.Serialize(id);

        Assert.Equal("\"abc-123\"", json);
    }

    [Fact]
    public void MessageID_DeserializesFromBareString()
    {
        var id = JsonSerializer.Deserialize<MessageID>("\"abc-123\"");

        Assert.NotNull(id);
        Assert.Equal("abc-123", id!.Value);
    }

    [Fact]
    public void MessageID_DeserializesNullAsNull()
    {
        var id = JsonSerializer.Deserialize<MessageID>("null");

        Assert.Null(id);
    }

    [Fact]
    public void ContactID_SerializesAsBareString()
    {
        var id = new ContactID("11111111-1111-1111-1111-111111111111");

        var json = JsonSerializer.Serialize(id);

        Assert.Equal("\"11111111-1111-1111-1111-111111111111\"", json);
    }

    [Fact]
    public void GroupID_SerializesAsBareString()
    {
        var id = new GroupID("22222222-2222-2222-2222-222222222222");

        var json = JsonSerializer.Serialize(id);

        Assert.Equal("\"22222222-2222-2222-2222-222222222222\"", json);
    }

    [Fact]
    public void ReceivedID_SerializesAsBareString()
    {
        var id = new ReceivedID("33333333-3333-3333-3333-333333333333");

        var json = JsonSerializer.Serialize(id);

        Assert.Equal("\"33333333-3333-3333-3333-333333333333\"", json);
    }
}