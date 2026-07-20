using System.Net;
using TNZAPI.NET.Helpers;

namespace TNZAPI.NET.Demo.Api.Tests.Helpers;

// Shared setup for every controller test class: an HttpClient talking to the in-process
// WebApplicationFactory, and a one-line way to fake the SDK's outbound TNZ API response for the
// request under test. Resets HttpRequest.MessageHandler after each test — required since it's a
// process-wide static field (see AssemblyInfo.cs's DisableTestParallelization for the other half of
// making that safe).
public abstract class DemoApiTestBase : IClassFixture<DemoApiFactory>, IDisposable
{
    protected readonly HttpClient Client;

    protected DemoApiTestBase(DemoApiFactory factory)
    {
        Client = factory.CreateClient();
        HttpRequest.MessageHandler = new ThrowingHttpMessageHandler();
    }

    protected static FakeHttpMessageHandler FakeResponse(HttpStatusCode statusCode, string responseBody)
    {
        var handler = new FakeHttpMessageHandler(statusCode, responseBody);
        HttpRequest.MessageHandler = handler;
        return handler;
    }

    protected static T SetHandler<T>(T handler) where T : HttpMessageHandler
    {
        HttpRequest.MessageHandler = handler;
        return handler;
    }

    public void Dispose()
    {
        Client.Dispose();
        HttpRequest.MessageHandler = new ThrowingHttpMessageHandler();
        OnDispose();
    }

    // Override for any additional process-wide state a test class's endpoints mutate beyond
    // MessageHandler (e.g. SettingsControllerTests resetting TNZ_API_URL/TNZ_ALLOW_INSECURE_HTTP)
    // — every test class shares one process, so anything left dirty here leaks into later classes.
    protected virtual void OnDispose()
    {
    }
}