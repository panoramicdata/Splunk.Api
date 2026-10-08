using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The authenticated user (<c>authentication/current-context</c>).</summary>
public sealed class CurrentContext : UserProperties
{
	/// <summary>The authenticated user's name.</summary>
	[JsonPropertyName("username")]
	public string? Username { get; init; }
}
