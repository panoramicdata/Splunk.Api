namespace Splunk.Api;

/// <summary>
/// Configuration for <see cref="SplunkHecClient"/>. The values are read once when the client is constructed; changing this
/// object afterwards does not affect an existing client.
/// </summary>
public class SplunkHecClientOptions : SplunkConnectionOptions
{
	/// <summary>
	/// Absolute URL of the HTTP Event Collector, e.g. <c>https://splunk.example.com:8088</c>. This is a different port
	/// (and often a different host, such as a load balancer) from the management port <see cref="SplunkClient"/> uses.
	/// </summary>
	public string BaseUrl { get; set; } = string.Empty;

	/// <summary>The HTTP Event Collector token, sent as <c>Authorization: Splunk {token}</c>.</summary>
	public string Token { get; set; } = string.Empty;

	/// <summary>
	/// The default channel (a GUID) sent as <c>X-Splunk-Request-Channel</c> on every request. Tokens with indexer
	/// acknowledgement (<c>useACK</c>) require a channel on every data request; a request whose options set a
	/// <c>channel</c> query parameter uses that instead. Leave <see langword="null"/> to send no channel header.
	/// </summary>
	public string? Channel { get; set; }

	internal void Validate()
	{
		if (!TryParseBaseUrl(BaseUrl, out _))
		{
			throw new ArgumentException("BaseUrl must be an absolute http or https URL.", nameof(BaseUrl));
		}

		if (string.IsNullOrWhiteSpace(Token))
		{
			throw new ArgumentException("Set Token to an HTTP Event Collector token.", nameof(Token));
		}

		if (Channel is not null && !Guid.TryParse(Channel, out _))
		{
			throw new ArgumentException("Channel must be a GUID.", nameof(Channel));
		}

		ValidateConnection();
	}

	/// <inheritdoc />
	public override string ToString()
		=> $"SplunkHecClientOptions {{ BaseUrl = {BaseUrl}, Token = {(string.IsNullOrEmpty(Token) ? "(none)" : "***")}, Channel = {Channel ?? "(none)"} }}";
}
