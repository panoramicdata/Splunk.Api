using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models;

/// <summary>The access control list of a Splunk object.</summary>
public sealed class SplunkAcl
{
	/// <summary>The app the object belongs to.</summary>
	[JsonPropertyName("app")]
	public string? App { get; init; }

	/// <summary>The user that owns the object, or <c>nobody</c>.</summary>
	[JsonPropertyName("owner")]
	public string? Owner { get; init; }

	/// <summary>The sharing level: <c>user</c>, <c>app</c>, <c>global</c> or <c>system</c>.</summary>
	[JsonPropertyName("sharing")]
	public string? Sharing { get; init; }

	/// <summary>The roles that can read and write the object.</summary>
	[JsonPropertyName("perms")]
	public SplunkPermissions? Permissions { get; init; }

	/// <summary>Whether the current user can change the object's permissions.</summary>
	[JsonPropertyName("can_change_perms")]
	public bool? CanChangePermissions { get; init; }

	/// <summary>Whether the current user can list the object.</summary>
	[JsonPropertyName("can_list")]
	public bool? CanList { get; init; }

	/// <summary>Whether the object can be shared at app level.</summary>
	[JsonPropertyName("can_share_app")]
	public bool? CanShareApp { get; init; }

	/// <summary>Whether the object can be shared globally.</summary>
	[JsonPropertyName("can_share_global")]
	public bool? CanShareGlobal { get; init; }

	/// <summary>Whether the object can be kept private to its owner.</summary>
	[JsonPropertyName("can_share_user")]
	public bool? CanShareUser { get; init; }

	/// <summary>Whether the current user can write the object.</summary>
	[JsonPropertyName("can_write")]
	public bool? CanWrite { get; init; }

	/// <summary>Whether the object can be removed.</summary>
	[JsonPropertyName("removable")]
	public bool? Removable { get; init; }

	/// <summary>Whether the object can be modified.</summary>
	[JsonPropertyName("modifiable")]
	public bool? Modifiable { get; init; }

	/// <summary>Any other ACL fields Splunk returned.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
