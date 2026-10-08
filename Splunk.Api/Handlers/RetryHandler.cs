using Microsoft.Extensions.Logging;
using System.Net;

namespace Splunk.Api.Handlers;

/// <summary>
/// Applies the per-attempt timeout and retries transient failures. A request is retried (up to
/// <see cref="SplunkClientOptions.MaxRetries"/> times, honouring <c>Retry-After</c>, else exponential back-off) only when its
/// body is replayable and either the status is 429 or 503, or the status is another 5xx and the verb is idempotent.
/// Only the method and path are logged, never the query string. The options are copied at construction.
/// </summary>
internal sealed class RetryHandler(SplunkClientOptions options) : DelegatingHandler
{
	private static readonly HashSet<HttpMethod> IdempotentMethods =
		[HttpMethod.Get, HttpMethod.Head, HttpMethod.Put, HttpMethod.Delete, HttpMethod.Options, HttpMethod.Trace];

	// The options are read once, here: changing the options object later does not affect a client that already exists.
	private readonly ILogger? _logger = options.Logger;
	private readonly TimeSpan _timeout = options.Timeout;
	private readonly int _maxRetries = options.MaxRetries;
	private readonly TimeSpan _retryBaseDelay = options.RetryBaseDelay;
	private readonly TimeSpan _maxRetryDelay = options.MaxRetryDelay;

	internal Func<TimeSpan, CancellationToken, Task> Delay { get; set; } = Task.Delay;

	/// <inheritdoc />
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		// Only the path is logged: query strings can carry search text.
		var path = request.RequestUri!.GetLeftPart(UriPartial.Path);
		var replayable = IsReplayable(request.Content);
		var backoff = _retryBaseDelay;
		var attempt = 0;
		var response = await SendAttemptAsync(request, path, attempt, cancellationToken).ConfigureAwait(false);
		while (replayable && attempt < _maxRetries && IsRetryable(request.Method, response.StatusCode))
		{
			var wait = CapDelay(RetryAfter(response) ?? backoff);
			if (_logger is not null)
			{
				Log.Retrying(_logger, (int)response.StatusCode, request.Method, path, wait);
			}

			response.Dispose();
			await Delay(wait, cancellationToken).ConfigureAwait(false);
			backoff = NextBackoff(backoff);
			attempt++;
			response = await SendAttemptAsync(request, path, attempt, cancellationToken).ConfigureAwait(false);
		}

		return response;
	}

	private async Task<HttpResponseMessage> SendAttemptAsync(HttpRequestMessage request, string path, int attempt, CancellationToken cancellationToken)
	{
		if (_logger is not null)
		{
			Log.Sending(_logger, request.Method, path, attempt + 1);
		}

		using var attemptCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		attemptCts.CancelAfter(_timeout);
		try
		{
			return await base.SendAsync(request, attemptCts.Token).ConfigureAwait(false);
		}
		catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested && attemptCts.IsCancellationRequested)
		{
			throw new TimeoutException($"Splunk did not respond within {_timeout}.");
		}
	}

	private TimeSpan CapDelay(TimeSpan wait) => wait > _maxRetryDelay ? _maxRetryDelay : wait;

	// Doubling is capped so it can never overflow TimeSpan.
	private TimeSpan NextBackoff(TimeSpan backoff) => backoff > _maxRetryDelay / 2 ? _maxRetryDelay : backoff * 2;

	/// <summary>
	/// Whether the body can be sent again unchanged: none, or buffered content (form fields and JSON bodies are both
	/// buffered). Streams are not, since they may be read-once.
	/// </summary>
	internal static bool IsReplayable(HttpContent? content)
		=> content is null or ByteArrayContent or ReadOnlyMemoryContent;

	/// <summary>
	/// 429 and 503 mean the request was not processed, so any verb is retried. Other 5xx may follow a partial or complete
	/// write, so only idempotent verbs are retried on them (never POST).
	/// </summary>
	internal static bool IsRetryable(HttpMethod method, HttpStatusCode status)
		=> status is HttpStatusCode.TooManyRequests or HttpStatusCode.ServiceUnavailable
			|| ((int)status >= 500 && IdempotentMethods.Contains(method));

	private static TimeSpan? RetryAfter(HttpResponseMessage response)
	{
		var header = response.Headers.RetryAfter;
		if (header?.Delta is { } delta)
		{
			return delta;
		}

		if (header?.Date is { } date)
		{
			var wait = date - DateTimeOffset.UtcNow;
			return wait > TimeSpan.Zero ? wait : TimeSpan.Zero;
		}

		return null;
	}
}
