using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>The number of recent app downloads from the deployment server (<c>deployment/server/clients/countRecentDownloads</c>).</summary>
public sealed class DeploymentDownloadCount : SplunkContent
{
	/// <summary>The number of downloads in the period asked for.</summary>
	[JsonPropertyName("count")]
	public int Count { get; init; }
}
