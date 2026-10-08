using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>One parameter of a modular input kind.</summary>
public sealed class ModularInputArgument
{
	/// <summary>The parameter's label.</summary>
	[JsonPropertyName("title")]
	public string? Title { get; init; }

	/// <summary>A description of the parameter.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The parameter's type, for example <c>string</c>, <c>number</c> or <c>boolean</c> (<c>data_type</c>).</summary>
	[JsonPropertyName("data_type")]
	public string? DataType { get; init; }

	/// <summary>The position in which the parameter is shown.</summary>
	[JsonPropertyName("order")]
	public int? Order { get; init; }

	/// <summary>Whether the parameter must be given when an input is created (<c>required_on_create</c>).</summary>
	[JsonPropertyName("required_on_create")]
	public bool? RequiredOnCreate { get; init; }

	/// <summary>Whether the parameter must be given when an input is edited (<c>required_on_edit</c>).</summary>
	[JsonPropertyName("required_on_edit")]
	public bool? RequiredOnEdit { get; init; }
}
