using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>The global forwarding settings (<c>data/outputs/tcp/default</c>).</summary>
	public ITcpOutputDefaults TcpOutputDefaults => field ??= For<ITcpOutputDefaults>();

	/// <summary>Forwarding target groups (<c>data/outputs/tcp/group</c>).</summary>
	public ITcpOutputGroups TcpOutputGroups => field ??= For<ITcpOutputGroups>();

	/// <summary>The receivers this server forwards to (<c>data/outputs/tcp/server</c>).</summary>
	public ITcpOutputServers TcpOutputServers => field ??= For<ITcpOutputServers>();

	/// <summary>Syslog forwarding groups (<c>data/outputs/tcp/syslog</c>).</summary>
	public ISyslogOutputs SyslogOutputs => field ??= For<ISyslogOutputs>();
}
