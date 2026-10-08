using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>
/// Creates a forwarding target group (<c>POST data/outputs/tcp/group</c>). When no default group is set, Splunk makes
/// the new group the default, so this server starts forwarding to it.
/// </summary>
public sealed class TcpOutputGroupCreateRequest : TcpOutputGroupUpdateRequest
{
	/// <summary>The group's name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
