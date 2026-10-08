using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The deployment server configuration (<c>deployment/server/config</c>).</summary>
/// <remarks><see cref="SplunkContent.Disabled"/> reports whether the deployment server is disabled. Global whitelist and blacklist entries are in <see cref="SplunkContent.AdditionalProperties"/>.</remarks>
public sealed class DeploymentServerConfig : SplunkContent
{
	/// <summary>The number of downloads in progress.</summary>
	[JsonPropertyName("currentDownloads")]
	public int? CurrentDownloads { get; init; }

	/// <summary>When the server classes were loaded.</summary>
	[JsonPropertyName("loadTime")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LoadTime { get; init; }

	/// <summary>Where the deployment server keeps the content to deploy.</summary>
	[JsonPropertyName("repositoryLocation")]
	public string? RepositoryLocation { get; init; }
}
