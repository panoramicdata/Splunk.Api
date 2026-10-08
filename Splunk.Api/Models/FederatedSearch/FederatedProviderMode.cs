using System.Text.Json.Serialization;

namespace Splunk.Api.Models.FederatedSearch;

/// <summary>How a Federated Search for Splunk provider runs federated searches.</summary>
public enum FederatedProviderMode
{
	/// <summary>A value this library does not recognise.</summary>
	Unknown = 0,

	/// <summary>Standard mode: searches name federated indexes explicitly.</summary>
	[JsonStringEnumMemberName("standard")]
	Standard,

	/// <summary>Transparent mode: for migrating hybrid search setups to federated search.</summary>
	[JsonStringEnumMemberName("transparent")]
	Transparent
}
