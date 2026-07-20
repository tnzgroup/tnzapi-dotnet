namespace TNZAPI.NET.Demo.Api.Tests.Helpers;

// ThrowingHttpMessageHandler is the "no fake configured" default DemoApiTestBase installs between
// tests — its whole point is to fail loudly if a test forgets to call FakeResponse()/SetHandler(),
// but that path is never exercised by any controller test (since every one of them correctly
// configures a fake handler first). Covered directly here instead.
public class ThrowingHttpMessageHandlerTests
{
    [Fact]
    public async Task SendAsync_AlwaysThrowsInvalidOperationException()
    {
        using var client = new HttpClient(new ThrowingHttpMessageHandler());

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => client.GetAsync("https://example.invalid/anything"));

        Assert.Contains("No fake response configured", ex.Message);
    }

    [Fact]
    public void IsAssignableToHttpClientHandler()
    {
        // Must stay an HttpClientHandler subclass, not a plain HttpMessageHandler — production code
        // in SettingsController.GetSslVerification does `HttpRequest.MessageHandler is
        // HttpClientHandler` to report whether SSL verification is enabled, and this is the default
        // handler value between tests.
        Assert.IsAssignableFrom<HttpClientHandler>(new ThrowingHttpMessageHandler());
    }
}