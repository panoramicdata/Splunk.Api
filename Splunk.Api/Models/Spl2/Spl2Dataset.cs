using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Spl2;

/// <summary>
/// A dataset SPL2 can read (<c>orchestrator/v1/datasets</c>): an index, lookup, saved search, view, job or federated
/// dataset. Kind-specific properties (such as an index's <c>frozenTimePeriodInSecs</c>) are in
/// <see cref="AdditionalProperties"/>.
/// </summary>
public sealed class Spl2Dataset
{
	/// <summary>The dataset ID, for example <c>indexes.main</c>.</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>The dataset name, for example <c>main</c>.</summary>
	[JsonPropertyName("name")]
	public string? Name { get; init; }

	/// <summary>The SPL2 namespace, for example <c>~indexes</c>.</summary>
	[JsonPropertyName("namespace")]
	public string? Namespace { get; init; }

	/// <summary>The fully qualified resource name, for example <c>~indexes.main</c>.</summary>
	[JsonPropertyName("resourceName")]
	public string? ResourceName { get; init; }

	/// <summary>The kind, for example <c>index</c>, <c>lookup</c>, <c>savedsearch</c>, <c>view</c> or <c>job</c>.</summary>
	[JsonPropertyName("kind")]
	public string? Kind { get; init; }

	/// <summary>The module the dataset is defined in, if any.</summary>
	[JsonPropertyName("module")]
	public string? Module { get; init; }

	/// <summary>The owner.</summary>
	[JsonPropertyName("owner")]
	public string? Owner { get; init; }

	/// <summary>The title.</summary>
	[JsonPropertyName("title")]
	public string? Title { get; init; }

	/// <summary>A summary.</summary>
	[JsonPropertyName("summary")]
	public string? Summary { get; init; }

	/// <summary>The description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The internal name.</summary>
	[JsonPropertyName("internalName")]
	public string? InternalName { get; init; }

	/// <summary>For an index, its data type: <c>event</c> or <c>metric</c>.</summary>
	[JsonPropertyName("datatype")]
	public string? DataType { get; init; }

	/// <summary>Who created the dataset.</summary>
	[JsonPropertyName("createdBy")]
	public string? CreatedBy { get; init; }

	/// <summary>When the dataset was created, as Splunk formats it.</summary>
	[JsonPropertyName("createdAt")]
	public string? CreatedAt { get; init; }

	/// <summary>Who last changed the dataset.</summary>
	[JsonPropertyName("updatedBy")]
	public string? UpdatedBy { get; init; }

	/// <summary>When the dataset was last changed, as Splunk formats it.</summary>
	[JsonPropertyName("updatedAt")]
	public string? UpdatedAt { get; init; }

	/// <summary>Every other property, including the deprecated lower-case duplicates (<c>createdby</c>, <c>resourcename</c>).</summary>
	[JsonExtensionData]
	public IDictionary<string, JsonElement> AdditionalProperties { get; init; } = new Dictionary<string, JsonElement>();
}
