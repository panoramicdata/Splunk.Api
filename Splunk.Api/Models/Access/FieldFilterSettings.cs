using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The settings of a field filter, shared by <see cref="FieldFilterCreateRequest"/> and <see cref="FieldFilterUpdateRequest"/>.</summary>
public abstract class FieldFilterSettings : SplunkFormRequest
{
	/// <summary>The one field the filter acts on (<c>action.field</c>).</summary>
	[JsonPropertyName("action.field")]
	public string? ActionField { get; init; }

	/// <summary>
	/// What the filter does to the field (<c>action.operator</c>): <c>null()</c>, <c>sha256()</c>, <c>sha512()</c>, or a
	/// replacement string in double quotes.
	/// </summary>
	[JsonPropertyName("action.operator")]
	public string? ActionOperator { get; init; }

	/// <summary>The filter's description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The indexes the filter applies to, separated by commas; omit for every index.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>Limits the filter to events with this <c>host</c>, <c>source</c> or <c>sourcetype</c> (<c>limit.key</c>).</summary>
	[JsonPropertyName("limit.key")]
	public string? LimitKey { get; init; }

	/// <summary>The values of <see cref="LimitKey"/>, each in double quotes and separated by commas (<c>limit.value</c>).</summary>
	[JsonPropertyName("limit.value")]
	public string? LimitValue { get; init; }

	/// <summary>The roles exempt from the filter, separated by commas.</summary>
	[JsonPropertyName("roleExemptions")]
	public string? RoleExemptions { get; init; }
}
