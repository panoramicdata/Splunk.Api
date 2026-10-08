using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>A field filter (<c>authorization/fieldfilters</c>).</summary>
public sealed class FieldFilter : SplunkContent
{
	/// <summary>The field the filter acts on (<c>action.field</c>).</summary>
	[JsonPropertyName("action.field")]
	public string? ActionField { get; init; }

	/// <summary>
	/// What the filter does to the field (<c>action.operator</c>): <c>null()</c> removes the value, <c>sha256()</c> or
	/// <c>sha512()</c> hash it, or a quoted string replaces it.
	/// </summary>
	[JsonPropertyName("action.operator")]
	public string? ActionOperator { get; init; }

	/// <summary>The filter's description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The indexes the filter applies to, separated by commas; empty for every index.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>Limits the filter to events with this <c>host</c>, <c>source</c> or <c>sourcetype</c> (<c>limit.key</c>).</summary>
	[JsonPropertyName("limit.key")]
	public string? LimitKey { get; init; }

	/// <summary>The values of <see cref="LimitKey"/> the filter applies to, separated by commas (<c>limit.value</c>).</summary>
	[JsonPropertyName("limit.value")]
	public string? LimitValue { get; init; }

	/// <summary>The roles exempt from the filter, as Splunk returns them (a list in JSON text, or comma-separated).</summary>
	[JsonPropertyName("roleExemptions")]
	public string? RoleExemptions { get; init; }
}
