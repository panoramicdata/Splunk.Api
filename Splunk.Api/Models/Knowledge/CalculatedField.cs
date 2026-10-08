using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A calculated field: an <c>EVAL-</c> expression in <c>props.conf</c> (<c>data/props/calcfields</c>).</summary>
/// <remarks><see cref="PropsEntry.Value"/> is the eval expression and <see cref="PropsEntry.Type"/> is always <c>EVAL</c>.</remarks>
public sealed class CalculatedField : PropsEntry
{
	/// <summary>The name of the field the expression calculates (<c>field.name</c>).</summary>
	[JsonPropertyName("field.name")]
	public string? FieldName { get; init; }
}
