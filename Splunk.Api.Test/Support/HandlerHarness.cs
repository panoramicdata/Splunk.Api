namespace Splunk.Api.Test.Support;

/// <summary>Sends requests through one delegating handler under test, with a <see cref="StubHandler"/> behind it.</summary>
internal sealed class HandlerHarness : IDisposable
{
	private readonly HttpMessageInvoker _invoker;

	public HandlerHarness(DelegatingHandler handler, HttpMessageHandler? inner = null)
	{
		handler.InnerHandler = inner ?? Stub;
		_invoker = new HttpMessageInvoker(handler);
	}

	public StubHandler Stub { get; } = new();

	public Task<HttpResponseMessage> SendAsync(HttpMethod method, string uri, HttpContent? content = null)
		=> SendAsync(new HttpRequestMessage(method, uri) { Content = content });

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
		=> _invoker.SendAsync(request, TestContext.Current.CancellationToken);

	public void Dispose() => _invoker.Dispose();
}
