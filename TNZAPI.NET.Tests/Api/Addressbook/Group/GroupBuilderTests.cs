using TNZAPI.NET.Api.Addressbook.Group;
using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Api.Addressbook.Group;

public class GroupBuilderTests
{
    [Fact]
    public void Build_SetsGroupNameAndSegmentationFields()
    {
        using var builder = new GroupBuilder();

        var model = builder
            .SetGroupName("VIP Customers")
            .SetSubAccount("Business Unit One")
            .SetDepartment("Team Alpha")
            .Build();

        Assert.Equal("VIP Customers", model.GroupName);
        Assert.Equal("Business Unit One", model.SubAccount);
        Assert.Equal("Team Alpha", model.Department);
    }

    [Fact]
    public void Build_SetsViewEditByAndAccessControl()
    {
        using var builder = new GroupBuilder();

        var model = builder
            .SetGroupName("VIP")
            .SetViewEditBy(Enums.ViewEditByOptions.Department)
            .SetAccessControl(Enums.AccessControlLevel.Limited)
            .Build();

        Assert.Equal(Enums.ViewEditByOptions.Department, model.ViewEditBy);
        Assert.Equal(Enums.AccessControlLevel.Limited, model.AccessControl);
    }
}