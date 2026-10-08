using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Inputs;

/// <summary>Changes the SSL settings of TCP inputs (<c>POST data/inputs/tcp/ssl/{name}</c>). Unset properties are left unchanged.</summary>
public sealed class TcpSslSettingsUpdateRequest : SplunkFormRequest
{
	/// <summary>Whether SSL inputs are disabled.</summary>
	[JsonPropertyName("disabled")]
	public bool? Disabled { get; init; }

	/// <summary>The server certificate's password.</summary>
	[JsonPropertyName("password")]
	public string? Password { get; init; }

	/// <summary>Whether clients must present a certificate (<c>requireClientCert</c>).</summary>
	[JsonPropertyName("requireClientCert")]
	public bool? RequireClientCert { get; init; }

	/// <summary>The root certificate authority file (<c>rootCA</c>).</summary>
	[JsonPropertyName("rootCA")]
	public string? RootCa { get; init; }

	/// <summary>The server certificate's full path (<c>serverCert</c>).</summary>
	[JsonPropertyName("serverCert")]
	public string? ServerCert { get; init; }
}
