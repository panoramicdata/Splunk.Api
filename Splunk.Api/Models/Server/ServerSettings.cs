using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>The server's general settings (<c>server/settings</c>).</summary>
public sealed class ServerSettings : SplunkContent
{
	/// <summary>The Splunk installation directory (<c>SPLUNK_HOME</c>).</summary>
	[JsonPropertyName("SPLUNK_HOME")]
	public string? SplunkHome { get; init; }

	/// <summary>The default index storage directory (<c>SPLUNK_DB</c>).</summary>
	[JsonPropertyName("SPLUNK_DB")]
	public string? SplunkDb { get; init; }

	/// <summary>The server name (<c>serverName</c>).</summary>
	[JsonPropertyName("serverName")]
	public string? ServerName { get; init; }

	/// <summary>The default host field value for events (<c>host</c>).</summary>
	[JsonPropertyName("host")]
	public string? Host { get; init; }

	/// <summary>The resolved host name (<c>host_resolved</c>).</summary>
	[JsonPropertyName("host_resolved")]
	public string? HostResolved { get; init; }

	/// <summary>The Splunk Web port (<c>httpport</c>).</summary>
	[JsonPropertyName("httpport")]
	public int? HttpPort { get; init; }

	/// <summary>The management (REST) port (<c>mgmtHostPort</c>).</summary>
	[JsonPropertyName("mgmtHostPort")]
	public int? ManagementPort { get; init; }

	/// <summary>The KV store port (<c>kvStorePort</c>).</summary>
	[JsonPropertyName("kvStorePort")]
	public int? KvStorePort { get; init; }

	/// <summary>Whether the KV store is disabled (<c>kvStoreDisabled</c>).</summary>
	[JsonPropertyName("kvStoreDisabled")]
	public bool? KvStoreDisabled { get; init; }

	/// <summary>Whether Splunk Web serves HTTPS (<c>enableSplunkWebSSL</c>).</summary>
	[JsonPropertyName("enableSplunkWebSSL")]
	public bool? EnableSplunkWebSsl { get; init; }

	/// <summary>Whether Splunk Web starts (<c>startwebserver</c>).</summary>
	[JsonPropertyName("startwebserver")]
	public bool? StartWebServer { get; init; }

	/// <summary>The minimum free disk space in megabytes before indexing stops (<c>minFreeSpace</c>).</summary>
	[JsonPropertyName("minFreeSpace")]
	public long? MinFreeSpaceMB { get; init; }

	/// <summary>The session timeout, for example <c>1h</c> (<c>sessionTimeout</c>).</summary>
	[JsonPropertyName("sessionTimeout")]
	public string? SessionTimeout { get; init; }

	/// <summary>The encrypted shared secret (<c>pass4SymmKey</c>).</summary>
	[JsonPropertyName("pass4SymmKey")]
	public string? Pass4SymmKey { get; init; }

	/// <summary>The IP address trusted to supply a user name for single sign-on (<c>trustedIP</c>).</summary>
	[JsonPropertyName("trustedIP")]
	public string? TrustedIP { get; init; }

	/// <summary>The application server ports (<c>appServerPorts</c>).</summary>
	[JsonPropertyName("appServerPorts")]
	public IReadOnlyList<int> AppServerPorts { get; init; } = [];
}
