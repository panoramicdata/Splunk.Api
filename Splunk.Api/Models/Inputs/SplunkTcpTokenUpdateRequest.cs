using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes a receiver token's value (<c>POST data/inputs/tcp/splunktcptoken/{name}</c>).</summary>
public sealed class SplunkTcpTokenUpdateRequest : SplunkFormRequest
{
	/// <summary>The new token value.</summary>
	[JsonPropertyName("token")]
	public required string Token { get; init; }
}
