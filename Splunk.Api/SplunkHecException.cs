using System.Net;

namespace Splunk.Api;

/// <summary>
/// Raised when the HTTP Event Collector rejects a request. It answers <c>{"text":"...","code":N}</c>; see Splunk's
/// "Possible error codes" for the meaning of <see cref="Code"/> (for example 4 for an invalid token, 10 for a missing
/// channel, 9 for a busy server).
/// </summary>
/// <param name="statusCode">The HTTP status code.</param>
/// <param name="code">The collector's status code, or <see langword="null"/> when the body held none.</param>
/// <param name="invalidEventNumber">The zero-based index of the first invalid event in a batch, where Splunk reports it.</param>
/// <param name="message">The collector's text, or a fallback naming the status.</param>
public sealed class SplunkHecException(HttpStatusCode statusCode, int? code, int? invalidEventNumber, string message)
	: Exception(message)
{
	/// <summary>The HTTP status code.</summary>
	public HttpStatusCode StatusCode { get; } = statusCode;

	/// <summary>The collector's status code (<c>code</c>), or <see langword="null"/> when the body held none.</summary>
	public int? Code { get; } = code;

	/// <summary>
	/// The zero-based index of the first invalid event in a batch (<c>invalid-event-number</c>); the events before it
	/// were accepted.
	/// </summary>
	public int? InvalidEventNumber { get; } = invalidEventNumber;
}
