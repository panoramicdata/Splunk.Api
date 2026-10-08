using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>An SPL2 module (<c>orchestrator/v1/spl2/modules</c>): named SPL2 statements in a namespace.</summary>
public sealed class Spl2Module
{
	/// <summary>The module name.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>The namespace, for example <c>apps.search</c>.</summary>
	[JsonPropertyName("namespace")]
	public string? Namespace { get; init; }

	/// <summary>The module's SPL2 source; omitted from lists unless definitions are requested.</summary>
	[JsonPropertyName("definition")]
	public string? Definition { get; init; }

	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The module's path in its app.</summary>
	[JsonPropertyName("sourcePath")]
	public string? SourcePath { get; init; }

	/// <summary>The version, incremented on each update.</summary>
	[JsonPropertyName("version")]
	public int? Version { get; init; }

	/// <summary>Whether the caller can change the module.</summary>
	[JsonPropertyName("canWrite")]
	public bool? CanWrite { get; init; }

	/// <summary>Who created the module.</summary>
	[JsonPropertyName("createdBy")]
	public string? CreatedBy { get; init; }

	/// <summary>When the module was created, as Splunk formats it.</summary>
	[JsonPropertyName("createdAt")]
	public string? CreatedAt { get; init; }

	/// <summary>Who last changed the module.</summary>
	[JsonPropertyName("updatedBy")]
	public string? UpdatedBy { get; init; }

	/// <summary>When the module was last changed, as Splunk formats it.</summary>
	[JsonPropertyName("updatedAt")]
	public string? UpdatedAt { get; init; }

	/// <summary>For a template, its template settings (<c>@template</c>): name, runtimes and source types.</summary>
	[JsonPropertyName("@template")]
	public JsonElement? Template { get; init; }

	/// <summary>Any other properties, such as annotations.</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
