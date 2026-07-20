using TNZAPI.NET.Api.Addressbook.Contact;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Addressbook.Contact;

public class ContactBuilderTests
{
    [Fact]
    public void Build_SetsNameAndContactFields()
    {
        using var builder = new ContactBuilder();

        var model = builder
            .SetFirstName("John")
            .SetLastName("Doe")
            .SetEmailAddress("john.doe@example.com")
            .SetMobilePhone("+64211234567")
            .Build();

        Assert.Equal("John", model.FirstName);
        Assert.Equal("Doe", model.LastName);
        Assert.Equal("john.doe@example.com", model.EmailAddress);
        Assert.Equal("+64211234567", model.MobilePhone);
    }

    [Fact]
    public void Build_SetsViewByEditByAndAccessControl()
    {
        using var builder = new ContactBuilder();

        var model = builder
            .SetFirstName("John")
            .SetViewBy(Enums.ViewEditByOptions.Account)
            .SetEditBy(Enums.ViewEditByOptions.SubAccount)
            .SetAccessControl(Enums.AccessControlLevel.Granted)
            .Build();

        Assert.Equal(Enums.ViewEditByOptions.Account, model.ViewBy);
        Assert.Equal(Enums.ViewEditByOptions.SubAccount, model.EditBy);
        Assert.Equal(Enums.AccessControlLevel.Granted, model.AccessControl);
    }

    [Fact]
    public void Build_SetsCustomFields()
    {
        using var builder = new ContactBuilder();

        var model = builder
            .SetCustom1("Value1")
            .SetCustom2("Value2")
            .Build();

        Assert.Equal("Value1", model.Custom1);
        Assert.Equal("Value2", model.Custom2);
    }

    [Fact]
    public void Build_SetsDirectPhone()
    {
        using var builder = new ContactBuilder();

        var model = builder
            .SetFirstName("Jane")
            .SetDirectPhone("+64211111111")
            .Build();

        Assert.Equal("+64211111111", model.DirectPhone);
    }
}