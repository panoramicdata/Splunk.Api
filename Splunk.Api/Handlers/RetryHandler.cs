using Microsoft.Extensions.Logging;
using System.Net;

namespace Splunk.Api.Handlers;

/// <summary>
/// Applies the per-attempt timeout and retries transient failures. A request is retried (up to
/// <see cref="SplunkConnectionOptions.MaxRetries"/> times, honouring <c>Retry-After</c>, else exponential back-off) only when its
/// body is replayable and either the status is 429 or 503, the status is another 5xx and the verb is idempotent, or the
/// connection could not be established (refused, reset during the TLS handshake, name not resolved), when nothing was sent.
/// Only the method and path are logged, never the query string. The settings are copied at construction.
/// </summary>
/// <param name="logger">The logger, if any.</param>
/// <param name="timeout">The per-attempt timeout.</param>
/// <param name="maxRetries">The maximum number of retries.</param>
/// <param name="retryBaseDelay">The initial back-off, doubled on each retry.</param>
/// <param name="maxRetryDelay">The longest single wait before a retry.</param>
internal sealed class RetryHandler(ILogger? logger, TimeSpan timeout, int maxRetries, TimeSpan retryBaseDelay, TimeSpan maxRetryDelay)
	: DelegatingHandler
{
	private static readonly HashSet<HttpMethod> IdempotentMethods =
		[HttpMethod.Get, HttpMethod.Head, HttpMethod.Put, HttpMethod.Delete, HttpMethod.Options, HttpMethod.Trace];

	// The settings are read once, here: changing the options object later does not affect a client that already exists.
	private readonly ILogger? _logger = logger;
	private readonly TimeSpan _timeout = timeout;
	private readonly int _maxRetries = maxRetries;
	private readonly TimeSpan _retryBaseDelay = retryBaseDelay;
	private readonly TimeSpan _maxRetryDelay = maxRetryDelay;

	/// <summary>Creates a handler with the retry and timeout settings of <paramref name="options"/>.</summary>
	/// <param name="options">The client options.</param>
	public RetryHandler(SplunkClientOptions options)
		: this(options.Logger, options.Timeout, options.MaxRetries, options.RetryBaseDelay, options.MaxRetryDelay)
	{
	}

	internal Func<TimeSpan, CancellationToken, Task> Delay { get; set; } = Task.Delay;

	/// <inheritdoc />
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		// Only the path is logged: query strings can carry search text.
		var path = request.RequestUri!.GetLeftPart(UriPartial.Path);
		var retries = IsReplayable(request.Content) ? _maxRetries : 0;
		var backoff = _retryBaseDelay;
		var attempt = 0;
		for (; attempt < retries; attempt++)
		{
			TimeSpan wait;
			try
			{
				var response = await SendAttemptAsync(request, path, attempt, cancellationToken).ConfigureAwait(false);
				if (!IsRetryable(request.Method, response.StatusCode))
				{
					return response;
				}

				wait = CapDelay(RetryAfter(response) ?? backoff);
				LogRetry(response.StatusCode, request.Method, path, wait);
				response.Dispose();
			}
			catch (HttpRequestException exception) when (IsConnectionFailure(exception))
			{
				wait = CapDelay(backoff);
				LogConnectionRetry(exception.HttpRequestError, request.Method, path, wait);
			}

			await Delay(wait, cancellationToken).ConfigureAwait(false);
			backoff = NextBackoff(backoff);
		}

		// The last attempt (or the only one, for a body that cannot be replayed) returns or throws whatever happens.
		return await SendAttemptAsync(request, path, attempt, cancellationToken).ConfigureAwait(false);
	}

	private void LogRetry(HttpStatusCode status, HttpMethod method, string path, TimeSpan wait)
	{
		if (_logger is not null)
		{
			Log.Retrying(_logger, (int)status, method, path, wait);
		}
	}

	private void LogConnectionRetry(HttpRequestError error, HttpMethod method, string path, TimeSpan wait)
	{
		if (_logger is not null)
		{
			Log.RetryingConnection(_logger, error, method, path, wait);
		}
	}

	/// <summary>
	/// Whether the request failed while the connection was being established (including the TLS handshake and name
	/// resolution), so nothing was sent and any verb can safely be retried. Failures after the request went out are not.
	/// </summary>
	internal static bool IsConnectionFailure(HttpRequestException exception)
		=> exception.HttpRequestError is HttpRequestError.ConnectionError
			or HttpRequestError.SecureConnectionError
			or HttpRequestError.NameResolutionError;

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
		catch (OperationCanceledException exception) when (!cancellationToken.IsCancellationRequested && attemptCts.IsCancellationRequested)
		{
			throw new TimeoutException($"Splunk did not respond within {_timeout}.", exception);
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
