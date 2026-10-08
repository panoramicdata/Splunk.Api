using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// Creates or changes an ingest actions S3 destination (<c>POST data/ingest/rfsdestinations</c>). Other
/// <c>indexes.conf</c> and <c>outputs.conf</c> settings, such as <c>remote.s3.kms.key_id</c>, go in
/// <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
public sealed class IngestDestinationCreateRequest : SplunkFormRequest
{
	/// <summary>The destination's name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The bucket and folder, for example <c>s3://bucket/folder/</c>.</summary>
	[JsonPropertyName("path")]
	public required string Path { get; init; }

	/// <summary>A description of the destination.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>The S3 endpoint URL (<c>remote.s3.endpoint</c>).</summary>
	[JsonPropertyName("remote.s3.endpoint")]
	public string? Endpoint { get; init; }

	/// <summary>The access key (<c>remote.s3.access_key</c>).</summary>
	[JsonPropertyName("remote.s3.access_key")]
	public string? AccessKey { get; init; }

	/// <summary>The secret key (<c>remote.s3.secret_key</c>).</summary>
	[JsonPropertyName("remote.s3.secret_key")]
	public string? SecretKey { get; init; }

	/// <summary>The server-side encryption scheme (<c>remote.s3.encryption</c>).</summary>
	[JsonPropertyName("remote.s3.encryption")]
	public string? Encryption { get; init; }

	/// <summary>The compression applied to uploaded files, for example <c>gzip</c>.</summary>
	[JsonPropertyName("compression")]
	public string? Compression { get; init; }

	/// <summary>Whether events are dropped when an upload fails (<c>dropEventsOnUploadError</c>).</summary>
	[JsonPropertyName("dropEventsOnUploadError")]
	public bool? DropEventsOnUploadError { get; init; }

	/// <summary>Seconds after which a batch is uploaded (<c>batchTimeout</c>).</summary>
	[JsonPropertyName("batchTimeout")]
	public int? BatchTimeout { get; init; }

	/// <summary>The batch size in kilobytes that triggers an upload (<c>batchSizeThresholdKB</c>).</summary>
	[JsonPropertyName("batchSizeThresholdKB")]
	public long? BatchSizeThresholdKB { get; init; }

	/// <summary>A host to proxy the request to.</summary>
	[JsonPropertyName("target")]
	public string? Target { get; init; }
}
