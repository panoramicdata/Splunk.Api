using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>A token forwarders must present to a receiving port (<c>data/inputs/tcp/splunktcptoken</c>). The entry name is <c>splunktcptoken://{name}</c>.</summary>
public sealed class SplunkTcpToken : InputContent
{
	/// <summary>The token value.</summary>
	[JsonPropertyName("token")]
	public string? Token { get; init; }
}
