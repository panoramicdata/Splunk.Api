using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Server information (<c>server/info</c>).</summary>
	public IServerInfo ServerInfo => field ??= For<IServerInfo>();

	/// <summary>Indexes (<c>data/indexes</c>).</summary>
	public IIndexes Indexes => field ??= For<IIndexes>();

	/// <summary>Indexes with bucket-level sizes (<c>data/indexes-extended</c>).</summary>
	public IIndexesExtended IndexesExtended => field ??= For<IIndexesExtended>();

	/// <summary>Index volumes (<c>data/index-volumes</c>).</summary>
	public IIndexVolumes IndexVolumes => field ??= For<IIndexVolumes>();

	/// <summary>Acceleration summary disk usage (<c>data/summaries</c>).</summary>
	public IDataSummaries DataSummaries => field ??= For<IDataSummaries>();

	/// <summary>The health report (<c>server/health</c>).</summary>
	public IHealth Health => field ??= For<IHealth>();

	/// <summary>Health report settings (<c>server/health-config</c>).</summary>
	public IHealthConfig HealthConfig => field ??= For<IHealthConfig>();

	/// <summary>Indexer and search introspection (<c>server/introspection</c>).</summary>
	public IIntrospection Introspection => field ??= For<IIntrospection>();

	/// <summary>KV store introspection (<c>server/introspection/kvstore</c>).</summary>
	public IKvStoreIntrospection KvStoreIntrospection => field ??= For<IKvStoreIntrospection>();

	/// <summary>Server status (<c>server/status</c>).</summary>
	public IServerStatus ServerStatus => field ??= For<IServerStatus>();

	/// <summary>Host and process resource use (<c>server/status/resource-usage</c>).</summary>
	public IResourceUsage ResourceUsage => field ??= For<IResourceUsage>();

	/// <summary>Machine resources and operating system settings (<c>server/sysinfo</c>).</summary>
	public ISystemInfo SystemInfo => field ??= For<ISystemInfo>();

	/// <summary>Monitoring console bookmarks (<c>saved/bookmarks/monitoring_console</c>).</summary>
	public IMonitoringConsoleBookmarks MonitoringConsoleBookmarks => field ??= For<IMonitoringConsoleBookmarks>();
}
