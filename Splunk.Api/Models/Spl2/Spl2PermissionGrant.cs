using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>The roles granted one operation on an SPL2 module.</summary>
/// <param name="Operation">The operation (<c>operation</c>): <c>read</c>, <c>write</c> or <c>execute</c>.</param>
/// <param name="Roles">The roles granted it (<c>roles</c>).</param>
public sealed record Spl2PermissionGrant(
	[property: JsonPropertyName("operation")] string Operation,
	[property: JsonPropertyName("roles")] IReadOnlyList<string> Roles);
