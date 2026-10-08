using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Splunk.Api.IntegrationTest;

/// <summary>
/// Clients for a disposable Splunk that publishes its HTTP Event Collector port, configured from user secrets or
/// environment variables (section <c>SplunkHec</c>).
/// </summary>
/// <remarks>
/// The shared test instance does not publish the collector port, so collector tests, and writes that would change the
/// shared instance's global configuration (forwarding, SSL, collector settings, ingest actions), run against a second
/// container started for the purpose, for example:
/// <c>docker run -d --name splunk-api-hec-test -p 38088:8088 -p 38090:8089 -e SPLUNK_START_ARGS=--accept-license
/// -e SPLUNK_GENERAL_TERMS=--accept-sgt-current-at-splunk-com -e SPLUNK_PASSWORD=... -e SPLUNK_HEC_TOKEN=... splunk/splunk:latest</c>.
/// Settings: <c>SplunkHec:BaseUrl</c> (collector, e.g. <c>https://localhost:38088</c>), <c>SplunkHec:Token</c> (the
/// container's HEC token), <c>SplunkHec:ManagementUrl</c> (e.g. <c>https://localhost:38090</c>), <c>SplunkHec:Username</c>,
/// <c>SplunkHec:Password</c> and <c>SplunkHec:TrustedServerCertificateThumbprint</c> (SHA-256, shared by both ports).
/// A missing setting fails the tests with a message naming it; nothing is skipped.
/// </remarks>
public sealed class SplunkHecFixture : IDisposable
{
	private readonly Lazy<SplunkHecClient> _hec;
	private readonly Lazy<SplunkClient> _management;

	public SplunkHecFixture()
	{
		Configuration = new ConfigurationBuilder()
			.AddUserSecrets<SplunkHecFixture>()
			.AddEnvironmentVariables()
			.Build();
		_hec = new Lazy<SplunkHecClient>(() => new SplunkHecClient(CreateHecOptions(null)));
		_management = new Lazy<SplunkClient>(() => new SplunkClient(new SplunkClientOptions
		{
			BaseUrl = Required("SplunkHec:ManagementUrl"),
			Username = Required("SplunkHec:Username"),
			Password = Required("SplunkHec:Password"),
			TrustedServerCertificateThumbprint = Configuration["SplunkHec:TrustedServerCertificateThumbprint"]
		}));
	}

	public IConfiguration Configuration { get; }

	/// <summary>A collector client using the container's own HEC token.</summary>
	public SplunkHecClient Hec => _hec.Value;

	/// <summary>A management-port client for the same container, as <c>admin</c>.</summary>
	public SplunkClient Management => _management.Value;

	/// <summary>The container's HEC token.</summary>
	public string Token => Required("SplunkHec:Token");

	/// <summary>Collector options from configuration, adjusted by <paramref name="tweak"/>.</summary>
	public SplunkHecClientOptions CreateHecOptions(Action<SplunkHecClientOptions>? tweak)
	{
		var options = new SplunkHecClientOptions
		{
			BaseUrl = Required("SplunkHec:BaseUrl"),
			Token = Token,
			TrustedServerCertificateThumbprint = Configuration["SplunkHec:TrustedServerCertificateThumbprint"]
		};
		tweak?.Invoke(options);
		return options;
	}

	/// <summary>
	/// Sends a DELETE the library has no method for (the reference does not document it), to clean up after a test.
	/// </summary>
	public async Task DeleteUndocumentedAsync(string servicePath, CancellationToken cancellationToken)
	{
		var pinned = Configuration["SplunkHec:TrustedServerCertificateThumbprint"];
		using var handler = new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = (_, certificate, _, errors)
				=> errors == System.Net.Security.SslPolicyErrors.None
					|| string.Equals(certificate?.GetCertHashString(HashAlgorithmName.SHA256), pinned, StringComparison.OrdinalIgnoreCase)
		};
		using var http = new HttpClient(handler) { BaseAddress = new Uri(Required("SplunkHec:ManagementUrl")) };
		var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Required("SplunkHec:Username")}:{Required("SplunkHec:Password")}"));
		using var request = new HttpRequestMessage(HttpMethod.Delete, servicePath);
		request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
		using var response = await http.SendAsync(request, cancellationToken);
		response.EnsureSuccessStatusCode();
	}

	private string Required(string key)
		=> Configuration[key] is { Length: > 0 } value
			? value
			: throw new InvalidOperationException(
				$"Collector integration tests need '{key}'. Set it with 'dotnet user-secrets set {key} <value> --project Splunk.Api.IntegrationTest' "
				+ "for a disposable Splunk container that publishes its collector port (see SplunkHecFixture).");

	public void Dispose()
	{
		if (_hec.IsValueCreated)
		{
			_hec.Value.Dispose();
		}

		if (_management.IsValueCreated)
		{
			_management.Value.Dispose();
		}
	}
}
