using System.Collections.Concurrent;

namespace Splunk.Api.Test.Support;

/// <summary>
/// A thread-safe transport that answers each request with <paramref name="respond"/>, recording the requests and the
/// Authorization header each carried when it arrived. For concurrency and timing tests, where <see cref="StubHandler"/>'s
/// queue is not enough.
/// </summary>
internal sealed class DelegateHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> respond) : HttpMessageHandler
{
	private readonly ConcurrentQueue<(HttpRequestMessage Request, string? Authorization)> _requests = new();

	public IReadOnlyList<HttpRequestMessage> Requests => [.. _requests.Select(r => r.Request)];

	public IReadOnlyList<string?> Authorizations => [.. _requests.Select(r => r.Authorization)];

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		_requests.Enqueue((request, request.Headers.Authorization?.ToString()));
		return respond(request, cancellationToken);
	}
}
