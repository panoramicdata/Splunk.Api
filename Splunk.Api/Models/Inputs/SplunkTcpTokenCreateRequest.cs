using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Creates a receiver token (<c>POST data/inputs/tcp/splunktcptoken</c>).</summary>
public sealed class SplunkTcpTokenCreateRequest : SplunkFormRequest
{
	/// <summary>The token's name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The token value; Splunk generates one when unset.</summary>
	[JsonPropertyName("token")]
	public string? Token { get; init; }
}
