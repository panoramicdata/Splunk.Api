using Microsoft.Extensions.Configuration;

namespace Splunk.Api.IntegrationTest;

/// <summary>
/// A <see cref="SplunkClient"/> for a live Splunk instance, configured from user secrets or environment variables.
/// </summary>
/// <remarks>
/// Requires <c>Splunk:BaseUrl</c> and either <c>Splunk:Token</c> or <c>Splunk:Username</c> + <c>Splunk:Password</c>;
/// <c>Splunk:TrustedServerCertificateThumbprint</c> (SHA-256) trusts a self-signed certificate. Set them with, for example,
/// <c>dotnet user-secrets set Splunk:BaseUrl https://localhost:38089 --project Splunk.Api.IntegrationTest</c>, or as
/// <c>Splunk__BaseUrl</c> etc. in the environment. <c>docker/Start-SplunkTestInstance.ps1</c> starts a disposable Splunk
/// in Docker and sets them all. A missing setting fails the tests with a message naming it; nothing is skipped.
/// </remarks>
public sealed class SplunkFixture : IDisposable
{
	/// <summary>The prefix of every object the integration tests create, so leftovers are recognisable.</summary>
	internal const string Prefix = "splunk_api_it_";

	private readonly Lazy<SplunkClient> _client;

	public SplunkFixture()
	{
		Configuration = new ConfigurationBuilder()
			.AddUserSecrets<SplunkFixture>()
			.AddEnvironmentVariables()
			.Build();
		_client = new Lazy<SplunkClient>(() => new SplunkClient(CreateOptions()));
	}

	public IConfiguration Configuration { get; }

	/// <summary>A client authenticated as configured.</summary>
	public SplunkClient Client => _client.Value;

	/// <summary>Builds options from configuration, failing loudly when a required setting is missing.</summary>
	public SplunkClientOptions CreateOptions() => CreateOptions(static _ => { });

	/// <summary>Builds options from configuration, then lets <paramref name="tweak"/> change them.</summary>
	public SplunkClientOptions CreateOptions(Action<SplunkClientOptions> tweak)
	{
		var options = new SplunkClientOptions
		{
			BaseUrl = Required("Splunk:BaseUrl"),
			Token = Configuration["Splunk:Token"],
			Username = Configuration["Splunk:Username"],
			Password = Configuration["Splunk:Password"],
			TrustedServerCertificateThumbprint = Configuration["Splunk:TrustedServerCertificateThumbprint"],
		};
		if (string.IsNullOrWhiteSpace(options.Token) && (string.IsNullOrWhiteSpace(options.Username) || string.IsNullOrEmpty(options.Password)))
		{
			throw new InvalidOperationException(
				"Integration tests need Splunk:Token, or Splunk:Username and Splunk:Password. Set them with "
				+ "'dotnet user-secrets set Splunk:Username admin --project Splunk.Api.IntegrationTest' (and Splunk:Password), "
				+ "or run docker/Start-SplunkTestInstance.ps1.");
		}

		tweak(options);
		return options;
	}

	/// <summary>A unique, recognisable name for an object a test creates.</summary>
	public static string UniqueName(string what) => $"{Prefix}{what}_{Guid.NewGuid():N}"[..Math.Min(60, Prefix.Length + what.Length + 33)];

	private string Required(string key)
		=> Configuration[key] is { Length: > 0 } value
			? value
			: throw new InvalidOperationException(
				$"Integration tests need '{key}'. Set it with 'dotnet user-secrets set {key} <value> --project Splunk.Api.IntegrationTest', "
				+ "or run docker/Start-SplunkTestInstance.ps1.");

	public void Dispose()
	{
		if (_client.IsValueCreated)
		{
			_client.Value.Dispose();
		}
	}
}
