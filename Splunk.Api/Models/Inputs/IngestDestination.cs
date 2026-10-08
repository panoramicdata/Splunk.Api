using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// An ingest actions S3 destination (<c>data/ingest/rfsdestinations</c>), written to <c>outputs.conf</c> as
/// <c>[rfs:{name}]</c>. Splunk returns the keys as <c>&lt;hidden&gt;</c>.
/// </summary>
public sealed class IngestDestination : SplunkContent
{
	/// <summary>The batch size in kilobytes that triggers an upload (<c>batchSizeThresholdKB</c>).</summary>
	[JsonPropertyName("batchSizeThresholdKB")]
	public long? BatchSizeThresholdKB { get; init; }

	/// <summary>Seconds after which a batch is uploaded (<c>batchTimeout</c>).</summary>
	[JsonPropertyName("batchTimeout")]
	public int? BatchTimeout { get; init; }

	/// <summary>The compression applied to uploaded files, for example <c>gzip</c>.</summary>
	[JsonPropertyName("compression")]
	public string? Compression { get; init; }

	/// <summary>A description of the destination.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Whether events are dropped when an upload fails (<c>dropEventsOnUploadError</c>).</summary>
	[JsonPropertyName("dropEventsOnUploadError")]
	public bool? DropEventsOnUploadError { get; init; }

	/// <summary>The file format written, for example <c>ndjson</c>.</summary>
	[JsonPropertyName("format")]
	public string? Format { get; init; }

	/// <summary>The bucket and folder, for example <c>s3://bucket/folder/</c>.</summary>
	[JsonPropertyName("path")]
	public string? Path { get; init; }

	/// <summary>The access key, as Splunk reports it (<c>&lt;hidden&gt;</c>) (<c>remote.s3.access_key</c>).</summary>
	[JsonPropertyName("remote.s3.access_key")]
	public string? AccessKey { get; init; }

	/// <summary>The server-side encryption scheme (<c>remote.s3.encryption</c>).</summary>
	[JsonPropertyName("remote.s3.encryption")]
	public string? Encryption { get; init; }

	/// <summary>The S3 endpoint URL (<c>remote.s3.endpoint</c>).</summary>
	[JsonPropertyName("remote.s3.endpoint")]
	public string? Endpoint { get; init; }

	/// <summary>The secret key, as Splunk reports it (<c>&lt;hidden&gt;</c>) (<c>remote.s3.secret_key</c>).</summary>
	[JsonPropertyName("remote.s3.secret_key")]
	public string? SecretKey { get; init; }
}
