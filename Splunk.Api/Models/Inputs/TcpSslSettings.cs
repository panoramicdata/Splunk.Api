using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>The SSL settings shared by every TCP input (<c>data/inputs/tcp/ssl</c>). Splunk has a single entry, named <c>""</c>.</summary>
public sealed class TcpSslSettings : InputContent
{
	/// <summary>The accepted ciphers (<c>cipherSuite</c>).</summary>
	[JsonPropertyName("cipherSuite")]
	public string? CipherSuite { get; init; }

	/// <summary>The accepted TLS versions (<c>sslVersions</c>), for example <c>tls1.2, tls1.3</c>.</summary>
	[JsonPropertyName("sslVersions")]
	public string? SslVersions { get; init; }

	/// <summary>The elliptic curves offered (<c>ecdhCurves</c>).</summary>
	[JsonPropertyName("ecdhCurves")]
	public string? EcdhCurves { get; init; }

	/// <summary>Whether clients must present a certificate (<c>requireClientCert</c>).</summary>
	[JsonPropertyName("requireClientCert")]
	public bool? RequireClientCert { get; init; }

	/// <summary>The server certificate's path (<c>serverCert</c>).</summary>
	[JsonPropertyName("serverCert")]
	public string? ServerCert { get; init; }

	/// <summary>The root certificate authority file (<c>rootCA</c>).</summary>
	[JsonPropertyName("rootCA")]
	public string? RootCa { get; init; }

	/// <summary>Whether clients may renegotiate SSL (<c>allowSslRenegotiation</c>).</summary>
	[JsonPropertyName("allowSslRenegotiation")]
	public bool? AllowSslRenegotiation { get; init; }
}
