using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>
/// The HTTP Event Collector's global settings: the entry named <c>http</c> (<c>data/inputs/http/http</c>). While
/// <see cref="SplunkContent.Disabled"/> is <see langword="true"/>, no token accepts data.
/// </summary>
public sealed class HecSettings : InputContent
{
	/// <summary>The port the collector listens on, 8088 by default.</summary>
	[JsonPropertyName("port")]
	public int? Port { get; init; }

	/// <summary>Whether the collector uses HTTPS (<c>enableSSL</c>).</summary>
	[JsonPropertyName("enableSSL")]
	public bool? EnableSsl { get; init; }

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

	/// <summary>The TLS versions accepted, for example <c>tls1.2, tls1.3</c> (<c>sslVersions</c>).</summary>
	[JsonPropertyName("sslVersions")]
	public string? SslVersions { get; init; }

	/// <summary>The default allowed indexes for tokens.</summary>
	[JsonPropertyName("indexes")]
	public IReadOnlyList<string> Indexes { get; init; } = [];

	/// <summary>What the collector does when its queues fill (<c>backpressureState</c>), for example <c>disabled</c>.</summary>
	[JsonPropertyName("backpressureState")]
	public string? BackpressureState { get; init; }
}
