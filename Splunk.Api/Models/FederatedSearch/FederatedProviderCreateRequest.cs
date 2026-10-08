using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>A new federated provider (<c>POST data/federated/provider</c>).</summary>
/// <remarks>A Federated Search for Splunk provider (<see cref="FederatedProviderType.Splunk"/>, the only type Splunk Enterprise allows) also needs <see cref="Mode"/>, <see cref="HostPort"/>, <see cref="ServiceAccount"/> and <see cref="Password"/>; an Amazon S3 provider needs the <c>Aws*</c> properties and <see cref="Database"/>.</remarks>
public sealed class FederatedProviderCreateRequest : SplunkFormRequest
{
	/// <summary>The provider name.</summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The provider type.</summary>
	[JsonPropertyName("type")]
	public required FederatedProviderType Type { get; init; }

	/// <summary>The search mode (Federated Search for Splunk providers).</summary>
	[JsonPropertyName("mode")]
	public FederatedProviderMode? Mode { get; init; }

	/// <summary>The remote search head, as <c>host:port</c> (management port).</summary>
	[JsonPropertyName("hostPort")]
	public string? HostPort { get; init; }

	/// <summary>The service account on the remote search head.</summary>
	[JsonPropertyName("serviceAccount")]
	public string? ServiceAccount { get; init; }

	/// <summary>The service account's password.</summary>
	[JsonPropertyName("password")]
	public string? Password { get; init; }

	/// <summary>The app on the remote search head whose knowledge objects standard mode searches use.</summary>
	[JsonPropertyName("appContext")]
	public string? AppContext { get; init; }

	/// <summary>The 12-digit AWS account ID (Amazon S3 providers, Splunk Cloud Platform only).</summary>
	[JsonPropertyName("aws_account_id")]
	public string? AwsAccountId { get; init; }

	/// <summary>A comma-separated list of the AWS Glue tables the provider can read (Amazon S3 providers).</summary>
	[JsonPropertyName("aws_glue_tables_allowlist")]
	public string? AwsGlueTablesAllowlist { get; init; }

	/// <summary>A comma-separated list of the ARNs of the customer-managed AWS KMS keys that encrypt the data (Amazon S3 providers).</summary>
	[JsonPropertyName("aws_kms_keys_arn_allowlist")]
	public string? AwsKmsKeysArnAllowlist { get; init; }

	/// <summary>A comma-separated list of the Amazon S3 paths the provider can search (Amazon S3 providers).</summary>
	[JsonPropertyName("aws_s3_paths_allowlist")]
	public string? AwsS3PathsAllowlist { get; init; }

	/// <summary>The AWS Glue Data Catalog database (Amazon S3 providers).</summary>
	[JsonPropertyName("database")]
	public string? Database { get; init; }
}
