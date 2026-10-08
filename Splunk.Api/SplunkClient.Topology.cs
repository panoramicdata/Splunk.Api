using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>The deployment topology, node identities and trusted connections (<c>stack-explainer/v1</c>).</summary>
	public ITopology Topology => field ??= For<ITopology>();
}
