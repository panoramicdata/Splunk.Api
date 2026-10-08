using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Changes a data model's definition or acceleration (<c>POST datamodel/model/{name}</c>).</summary>
public sealed class DataModelUpdateRequest : SplunkFormRequest
{
	/// <summary>The new data model definition as JSON (<c>description</c>).</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The new acceleration settings as JSON (<c>acceleration</c>), for example <c>{"enabled":false}</c>.</summary>
	[JsonPropertyName("acceleration")]
	public string? Acceleration { get; init; }

	/// <summary>
	/// When <see langword="true"/>, Splunk validates the definition without saving it (<c>provisional</c>); the response
	/// then carries only the validated description.
	/// </summary>
	[JsonPropertyName("provisional")]
	public bool? Provisional { get; init; }
}
