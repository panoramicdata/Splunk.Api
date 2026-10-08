using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>The kind of <c>props.conf</c> field extraction to create.</summary>
public enum FieldExtractionType
{
	/// <summary>Not set.</summary>
	Unknown = 0,

	/// <summary>An inline regular expression with named capture groups (<c>EXTRACT-</c>).</summary>
	[JsonStringEnumMemberName("EXTRACT")]
	Extract,

	/// <summary>A reference to one or more <c>transforms.conf</c> stanzas (<c>REPORT-</c>).</summary>
	[JsonStringEnumMemberName("REPORT")]
	Report
}
