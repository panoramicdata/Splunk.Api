using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>A federated provider definition (<c>data/federated/provider</c>).</summary>
/// <remarks>Provider stanzas always live in <c>etc/system/local/federated.conf</c>, whatever namespace the request uses. <see cref="SplunkContent.Disabled"/> reports whether the provider is turned off.</remarks>
public sealed class FederatedProvider : SplunkContent
{
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

	/// <summary>The AWS region of the Splunk Cloud Platform deployment (set by Splunk).</summary>
	[JsonPropertyName("aws_region")]
	public string? AwsRegion { get; init; }

	/// <summary>A comma-separated list of the Amazon S3 paths the provider can search (Amazon S3 providers).</summary>
	[JsonPropertyName("aws_s3_paths_allowlist")]
	public string? AwsS3PathsAllowlist { get; init; }

	/// <summary>The result of the last connection check: <c>valid</c>, <c>invalid</c> or <c>unknown</c>. Diagnostic only.</summary>
	[JsonPropertyName("connectivityStatus")]
	public string? ConnectivityStatus { get; init; }

	/// <summary>The ARN of the AWS Glue Data Catalog (set by Splunk).</summary>
	[JsonPropertyName("data_catalog")]
	public string? DataCatalog { get; init; }

	/// <summary>The AWS Glue Data Catalog database (Amazon S3 providers).</summary>
	[JsonPropertyName("database")]
	public string? Database { get; init; }

	/// <summary>The federated indexes the provider may serve when index-based provider filtering is on; <c>*</c> for all.</summary>
	[JsonPropertyName("fedSrchIndexesAllowed")]
	public string? FederatedIndexesAllowed { get; init; }

	/// <summary>The remote search head, as <c>host:port</c> (management port).</summary>
	[JsonPropertyName("hostPort")]
	public string? HostPort { get; init; }

	/// <summary>The search mode (Federated Search for Splunk providers).</summary>
	[JsonPropertyName("mode")]
	public FederatedProviderMode Mode { get; init; }

	/// <summary>The service account used on the remote search head.</summary>
	[JsonPropertyName("serviceAccount")]
	public string? ServiceAccount { get; init; }

	/// <summary>The provider type.</summary>
	[JsonPropertyName("type")]
	public FederatedProviderType Type { get; init; }

	/// <summary>Whether the app context of the local search is used rather than <see cref="AppContext"/>.</summary>
	[JsonPropertyName("useAppContextFromSearch")]
	public bool? UseAppContextFromSearch { get; init; }

	/// <summary>Whether searches use knowledge objects replicated from the federated search head rather than the remote search head's own; always <c>false</c> for standard mode.</summary>
	[JsonPropertyName("useFSHKnowledgeObjects")]
	public bool? UseFshKnowledgeObjects { get; init; }
}
