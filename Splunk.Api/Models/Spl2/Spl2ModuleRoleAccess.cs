using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>The operations one role may perform on an SPL2 module (<c>GET orchestrator/v1/spl2/modules/permissions</c>).</summary>
public sealed class Spl2ModuleRoleAccess
{
	/// <summary>The resource type, <c>module</c>.</summary>
	[JsonPropertyName("resourceType")]
	public string? ResourceType { get; init; }

	/// <summary>The module's fully qualified name.</summary>
	[JsonPropertyName("resourceName")]
	public string? ResourceName { get; init; }

	/// <summary>The role.</summary>
	[JsonPropertyName("role")]
	public string? Role { get; init; }

	/// <summary>The operations the role may perform, for example <c>read</c>, <c>write</c> and <c>execute</c>.</summary>
	[JsonPropertyName("operations")]
	public IReadOnlyList<string> Operations { get; init; } = [];
}
