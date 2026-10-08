using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Outputs;

/// <summary>Changes a receiver (<c>POST data/outputs/tcp/server/{name}</c>). Unset properties are left unchanged.</summary>
public class TcpOutputServerUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether forwarding to the receiver is disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>How data is distributed: <c>clone</c>, <c>balance</c> or <c>autobalance</c>.</summary>
	[JsonPropertyName("method")]
	public string? Method { get; init; }

	/// <summary>The alternate name to match in the receiver's certificate (<c>sslAltNameToCheck</c>).</summary>
	[JsonPropertyName("sslAltNameToCheck")]
	public string? SslAltNameToCheck { get; init; }

	/// <summary>The client certificate's path; setting it turns SSL on (<c>sslCertPath</c>).</summary>
	[JsonPropertyName("sslCertPath")]
	public string? SslCertPath { get; init; }

	/// <summary>The SSL cipher list (<c>sslCipher</c>).</summary>
	[JsonPropertyName("sslCipher")]
	public string? SslCipher { get; init; }

	/// <summary>The common name to match in the receiver's certificate (<c>sslCommonNameToCheck</c>).</summary>
	[JsonPropertyName("sslCommonNameToCheck")]
	public string? SslCommonNameToCheck { get; init; }

	/// <summary>The client certificate's password (<c>sslPassword</c>).</summary>
	[JsonPropertyName("sslPassword")]
	public string? SslPassword { get; init; }

	/// <summary>The root certificate authority file (<c>sslRootCAPath</c>).</summary>
	[JsonPropertyName("sslRootCAPath")]
	public string? SslRootCaPath { get; init; }

	/// <summary>Whether the receiver's certificate is verified (<c>sslVerifyServerCert</c>).</summary>
	[JsonPropertyName("sslVerifyServerCert")]
	public bool? SslVerifyServerCert { get; init; }
}
