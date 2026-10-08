using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// Changes the HTTP Event Collector's global settings (<c>POST data/inputs/http/http</c>; pass <see cref="SettingsName"/>
/// as the name). Unset properties are left unchanged.
/// </summary>
public sealed class HecSettingsUpdateRequest : SplunkFormRequest
{
	/// <summary>The name of the entry that holds the global settings, <c>http</c>.</summary>
	public const string SettingsName = "http";

	/// <summary>Whether the whole collector is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>Whether the collector uses HTTPS (<c>enableSSL</c>).</summary>
	[JsonPropertyName("enableSSL")]
	public bool? EnableSsl { get; init; }

	/// <summary>The port the collector listens on.</summary>
	[JsonPropertyName("port")]
	public int? Port { get; init; }

	/// <summary>The number of I/O threads (<c>dedicatedIoThreads</c>).</summary>
	[JsonPropertyName("dedicatedIoThreads")]
	public int? DedicatedIoThreads { get; init; }

	/// <summary>The maximum simultaneous connections (<c>maxSockets</c>); 0 means automatic.</summary>
	[JsonPropertyName("maxSockets")]
	public int? MaxSockets { get; init; }

	/// <summary>The maximum threads for active transactions (<c>maxThreads</c>); 0 means automatic.</summary>
	[JsonPropertyName("maxThreads")]
	public int? MaxThreads { get; init; }

	/// <summary>Whether the configuration is written to a deployment server repository (<c>useDeploymentServer</c>).</summary>
	[JsonPropertyName("useDeploymentServer")]
	public bool? UseDeploymentServer { get; init; }

	/// <summary>The default index for tokens that do not set one.</summary>
	[JsonPropertyName("index")]
	public string? Index { get; init; }

	/// <summary>The default sourcetype for tokens that do not set one.</summary>
	[JsonPropertyName("sourcetype")]
	public string? Sourcetype { get; init; }
}
