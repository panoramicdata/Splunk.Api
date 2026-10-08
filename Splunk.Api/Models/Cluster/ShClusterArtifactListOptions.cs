using Refit;

namespace Splunk.Api.Models.Cluster;

/// <summary>The parameters of <c>GET shcluster/captain/artifacts</c>.</summary>
public sealed class ShClusterArtifactListOptions : ListOptions
{
	/// <summary>Whether to return the searches the captain sees, including ad hoc searches on members (documented as required).</summary>
	[AliasAs("remote_sids")]
	public bool? RemoteSids { get; init; }
}
