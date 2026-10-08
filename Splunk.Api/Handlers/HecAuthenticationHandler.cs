using System.Net.Http.Headers;

namespace Splunk.Api.Handlers;

/// <summary>
/// Sends the HTTP Event Collector token (<c>Authorization: Splunk {token}</c>) and, when configured, the default channel
/// (<c>X-Splunk-Request-Channel</c>) unless the request names its own channel in the query string.
/// </summary>
internal sealed class HecAuthenticationHandler(string token, string? channel) : DelegatingHandler
{
	internal const string ChannelHeader = "X-Splunk-Request-Channel";

	/// <inheritdoc />
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		request.Headers.Authorization = new AuthenticationHeaderValue("Splunk", token);
		if (channel is not null && !HasChannelParameter(request.RequestUri!))
		{
			request.Headers.Remove(ChannelHeader);
			request.Headers.Add(ChannelHeader, channel);
		}

		return base.SendAsync(request, cancellationToken);
	}

	private static bool HasChannelParameter(Uri uri)
		=> uri.Query.TrimStart('?').Split('&').Any(p => p.StartsWith("channel=", StringComparison.Ordinal));
}
