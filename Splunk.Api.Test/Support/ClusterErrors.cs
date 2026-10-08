namespace Splunk.Api.Test.Support;

/// <summary>The error bodies a Splunk 10.6 node without the clustering role asked for answers with (captured live).</summary>
internal static class ClusterErrors
{
	public const string ManagerNotEnabled = """{"messages":[{"type":"ERROR","text":"Cluster manager is not enabled on this node"}]}""";

	public const string PeerNotEnabled = """{"messages":[{"type":"ERROR","text":"Cluster peer is not enabled on this node, check clustering stanza in server.conf"}]}""";

	public const string SearchHeadNotEnabled = """{"messages":[{"type":"ERROR","text":"Searchhead is not enabled on this node"}]}""";

	public const string ShcNotEnabled = """{"messages":[{"type":"ERROR","text":"Search Head Clustering is not enabled on this node. REST endpoint is not available"}]}""";
}
