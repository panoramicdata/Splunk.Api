using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>Creates a role (<c>POST authorization/roles</c>).</summary>
public sealed class RoleCreateRequest : RoleSettings
{
	/// <summary>The role name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }
}
