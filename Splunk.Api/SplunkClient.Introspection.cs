using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Server information (<c>server/info</c>).</summary>
	public IServerInfo ServerInfo => field ??= For<IServerInfo>();
}
