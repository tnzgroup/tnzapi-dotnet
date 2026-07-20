using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class EnumsTests
{
    [Fact]
    public void ResultCode_HasRecordNotFound()
    {
        Assert.True(Enum.IsDefined(typeof(Enums.ResultCode), Enums.ResultCode.RecordNotFound));
    }

    [Fact]
    public void WebhookCallbackType_HasPostAndGet()
    {
        Assert.True(Enum.IsDefined(typeof(Enums.WebhookCallbackType), Enums.WebhookCallbackType.POST));
        Assert.True(Enum.IsDefined(typeof(Enums.WebhookCallbackType), Enums.WebhookCallbackType.GET));
    }
}