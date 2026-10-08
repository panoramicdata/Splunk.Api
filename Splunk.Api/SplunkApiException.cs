using Splunk.Api.Models;
using System.Net;

namespace Splunk.Api;

/// <summary>Raised when Splunk returns a non-success response.</summary>
/// <param name="statusCode">The HTTP status code.</param>
/// <param name="messages">The messages Splunk returned, in order; empty when the body held none.</param>
/// <param name="message">The exception message: Splunk's first error text, or a fallback naming the status.</param>
public sealed class SplunkApiException(HttpStatusCode statusCode, IReadOnlyList<SplunkMessage> messages, string message)
	: Exception(message)
{
	/// <summary>The HTTP status code.</summary>
	public HttpStatusCode StatusCode { get; } = statusCode;

	/// <summary>The messages Splunk returned (typically one with type <c>ERROR</c>); empty when the body held none.</summary>
	public IReadOnlyList<SplunkMessage> Messages { get; } = messages;
}
