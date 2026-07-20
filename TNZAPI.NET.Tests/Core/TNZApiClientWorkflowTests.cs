using TNZAPI.NET.Core;

namespace TNZAPI.NET.Tests.Core;

public class TNZApiClientWorkflowTests
{
    [Fact]
    public void Client_ExposesMessagingWorkflow()
    {
        var client = new TNZApiClient("test-token");

        Assert.NotNull(client.Messaging.Workflow);
    }
}