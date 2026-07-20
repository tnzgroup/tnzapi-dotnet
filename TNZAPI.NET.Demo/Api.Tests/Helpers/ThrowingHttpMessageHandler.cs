namespace TNZAPI.NET.Demo.Api.Tests.Helpers;

// The "no fake configured" state for HttpRequest.MessageHandler between/before tests. Without this,
// the handler falls back to a real HttpClientHandler, so a test that forgets to call
// FakeResponse()/SetHandler() would silently attempt a live outbound call to TNZ's real API instead
// of failing immediately with a clear setup error.
//
// Inherits HttpClientHandler (rather than the abstract HttpMessageHandler directly) because
// SettingsController.GetSslVerification does `HttpRequest.MessageHandler is HttpClientHandler` as
// its actual production logic for reporting whether SSL verification is enabled — subclassing keeps
// that type check true for this "unconfigured" default too, matching the real HttpClientHandler this
// replaces, while still failing fast on any actual dispatch.
public class ThrowingHttpMessageHandler : HttpClientHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw new InvalidOperationException("No fake response configured — did you forget to call FakeResponse() or SetHandler()?");
    }
}