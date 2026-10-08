using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>New credentials for a search peer (<c>POST search/distributed/peers/{name}</c>).</summary>
public sealed class DistributedPeerUpdateRequest : SplunkFormRequest
{
	/// <summary>An admin user on the peer, used to exchange keys.</summary>
	[JsonPropertyName("remoteUsername")]
	public required string RemoteUsername { get; init; }

	/// <summary>That user's password.</summary>
	[JsonPropertyName("remotePassword")]
	public required string RemotePassword { get; init; }
}
