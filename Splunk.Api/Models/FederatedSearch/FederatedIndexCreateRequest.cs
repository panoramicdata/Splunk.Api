using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>A new federated index (<c>POST data/federated/index</c>).</summary>
public sealed class FederatedIndexCreateRequest : SplunkFormRequest
{
	/// <summary>The index name; <c>federated:&lt;name&gt;</c> for Federated Search for Splunk (lowercase letters, digits, <c>_</c> and <c>-</c>).</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The federated provider that holds the dataset.</summary>
	[JsonPropertyName("federated.provider")]
	public required string Provider { get; init; }

	/// <summary>The remote dataset, as <c>&lt;prefix&gt;:&lt;name&gt;</c>, for example <c>index:main</c>.</summary>
	[JsonPropertyName("federated.dataset")]
	public required string Dataset { get; init; }

	/// <summary>The field of the AWS Glue table that acts as the event timestamp (Amazon S3 indexes only).</summary>
	[JsonPropertyName("federated.timefield")]
	public string? TimeField { get; init; }

	/// <summary>The <c>strptime</c> format of <see cref="TimeField"/> (Amazon S3 indexes only).</summary>
	[JsonPropertyName("federated.timeformat")]
	public string? TimeFormat { get; init; }

	/// <summary>An alias of <see cref="TimeField"/> converted to UNIX time at search time; defaults to <c>_time</c> (Amazon S3 indexes only).</summary>
	[JsonPropertyName("federated.unixtimefield")]
	public string? UnixTimeField { get; init; }

	/// <summary>A comma-separated list of the partition time fields, in partition level order (Amazon S3 indexes only).</summary>
	[JsonPropertyName("federated.partition.time.fields")]
	public string? PartitionTimeFields { get; init; }

	/// <summary>A comma-separated list of the time formats of <see cref="PartitionTimeFields"/> (Amazon S3 indexes only).</summary>
	[JsonPropertyName("federated.partition.time.formats")]
	public string? PartitionTimeFormats { get; init; }

	/// <summary>A comma-separated list of the types of <see cref="PartitionTimeFields"/>: <c>string</c>, <c>integer</c> or <c>date</c> (Amazon S3 indexes only).</summary>
	[JsonPropertyName("federated.partition.time.types")]
	public string? PartitionTimeTypes { get; init; }

	/// <summary>The canonical time zone of <see cref="PartitionTimeFields"/>, for example <c>America/Los_Angeles</c> (Amazon S3 indexes only).</summary>
	[JsonPropertyName("federated.partition.time.tz")]
	public string? PartitionTimeZone { get; init; }
}
