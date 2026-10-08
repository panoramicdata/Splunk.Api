using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>Replaces the permissions of an SPL2 module (<c>PUT orchestrator/v1/spl2/modules/permissions</c>), sent as JSON.</summary>
public sealed class Spl2ModulePermissionsRequest
{
	/// <summary>The resource type (<c>resourceType</c>), always <c>module</c>.</summary>
	[JsonPropertyName("resourceType")]
	public string ResourceType { get; init; } = "module";

	/// <summary>The module's fully qualified name (<c>resourceName</c>), for example <c>apps.search.my_module</c>.</summary>
	[JsonPropertyName("resourceName")]
	public required string ResourceName { get; init; }

	/// <summary>The roles granted each operation (<c>permissions</c>).</summary>
	[JsonPropertyName("permissions")]
	public required IReadOnlyList<Spl2PermissionGrant> Permissions { get; init; }
}
