using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>
/// The connection settings of a federated provider, shared by <see cref="FederatedProviderCreateRequest"/> and
/// <see cref="FederatedProviderUpdateRequest"/>.
/// </summary>
public abstract class FederatedProviderSettings : SplunkFormRequest
{
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
}
