using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>System messages (<c>messages</c>).</summary>
	public IMessages Messages => field ??= For<IMessages>();

	/// <summary>Server control actions such as restart (<c>server/control</c>).</summary>
	public IServerControl ServerControl => field ??= For<IServerControl>();

	/// <summary>splunkd's outbound HTTP proxy (<c>server/httpsettings/proxysettings</c>).</summary>
	public IProxySettings ProxySettings => field ??= For<IProxySettings>();

	/// <summary>splunkd logging categories (<c>server/logger</c>).</summary>
	public ILoggers Loggers => field ??= For<ILoggers>();

	/// <summary>The server's roles (<c>server/roles</c>).</summary>
	public IServerRoles ServerRoles => field ??= For<IServerRoles>();

	/// <summary>The server's general settings (<c>server/settings</c>).</summary>
	public IServerSettings ServerSettings => field ??= For<IServerSettings>();

	/// <summary>Server security operations (<c>server/security</c>).</summary>
	public IServerSecurity ServerSecurity => field ??= For<IServerSecurity>();
}
