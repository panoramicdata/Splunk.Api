using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>A new search peer (<c>POST search/distributed/peers</c>).</summary>
public sealed class DistributedPeerCreateRequest : SplunkFormRequest
{
	/// <summary>The peer, as <c>host:management_port</c>.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>An admin user on the peer, used to exchange keys.</summary>
	[JsonPropertyName("remoteUsername")]
	public required string RemoteUsername { get; init; }

	/// <summary>That user's password.</summary>
	[JsonPropertyName("remotePassword")]
	public required string RemotePassword { get; init; }
}
