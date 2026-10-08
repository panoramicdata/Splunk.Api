using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>The kind of a federated provider.</summary>
public enum FederatedProviderType
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>Federated Search for Splunk: the provider is another Splunk platform deployment.</summary>
	[JsonStringEnumMemberName("splunk")]
	Splunk,

	/// <summary>Federated Search for Amazon S3 (Splunk Cloud Platform only).</summary>
	[JsonStringEnumMemberName("aws_s3")]
	AwsS3
}
