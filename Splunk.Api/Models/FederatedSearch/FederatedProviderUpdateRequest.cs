using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>Changes to a federated provider (<c>POST data/federated/provider/{name}</c>). Only the properties set are sent; set at least one.</summary>
public sealed class FederatedProviderUpdateRequest : SplunkFormRequest
{
	/// <summary>The app on the remote search head whose knowledge objects standard mode searches use.</summary>
	[JsonPropertyName("appContext")]
	public string? AppContext { get; init; }

	/// <summary>The remote search head, as <c>host:port</c> (management port).</summary>
	[JsonPropertyName("hostPort")]
	public string? HostPort { get; init; }

	/// <summary>The service account on the remote search head.</summary>
	[JsonPropertyName("serviceAccount")]
	public string? ServiceAccount { get; init; }

	/// <summary>The service account's password.</summary>
	[JsonPropertyName("password")]
	public string? Password { get; init; }

	/// <summary>The federated indexes the provider may serve when index-based provider filtering is on.</summary>
	[JsonPropertyName("fedSrchIndexesAllowed")]
	public string? FederatedIndexesAllowed { get; init; }

	/// <summary>Whether to use the app context of the local search rather than <see cref="AppContext"/>.</summary>
	[JsonPropertyName("useAppContextFromSearch")]
	public bool? UseAppContextFromSearch { get; init; }

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
