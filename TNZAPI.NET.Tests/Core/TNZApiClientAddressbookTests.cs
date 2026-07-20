using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientAddressbookTests
{
    [Fact]
    public void Client_ExposesAddressbookContact()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Addressbook.Contact);
    }

    [Fact]
    public void Client_ExposesAddressbookGroup()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Addressbook.Group);
    }

    [Fact]
    public void Client_ExposesAddressbookContactGroupRelation()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Addressbook.Contact.Group);
    }

    [Fact]
    public void Client_ExposesAddressbookGroupContactRelation()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Addressbook.Group.Contact);
    }
}