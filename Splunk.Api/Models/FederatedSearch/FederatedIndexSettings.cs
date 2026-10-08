using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>
/// The optional settings of a federated index, shared by <see cref="FederatedIndexCreateRequest"/> and
/// <see cref="FederatedIndexUpdateRequest"/>.
/// </summary>
public abstract class FederatedIndexSettings : SplunkFormRequest
{
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
