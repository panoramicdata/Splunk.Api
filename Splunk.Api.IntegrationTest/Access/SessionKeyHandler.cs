using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace Splunk.Api.IntegrationTest.Access;

/// <summary>
/// A transport for a client configured with a session key as its token: it sends the key as
/// <c>Authorization: Splunk {key}</c> instead of <c>Bearer</c>, and trusts the configured certificate thumbprint.
/// </summary>
internal sealed class SessionKeyHandler : DelegatingHandler
{
	public SessionKeyHandler(string? thumbprint)
	{
		var pinned = thumbprint?.Replace(":", string.Empty, StringComparison.Ordinal);
		InnerHandler = new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = (_, certificate, _, errors)
				=> errors == System.Net.Security.SslPolicyErrors.None
					|| (certificate is not null && pinned is not null
						&& string.Equals(certificate.GetCertHashString(HashAlgorithmName.SHA256), pinned, StringComparison.OrdinalIgnoreCase))
		};
	}

	/// <summary>A client that authenticates every request with <paramref name="sessionKey"/>.</summary>
	public static SplunkClient CreateClient(SplunkFixture fixture, string sessionKey)
	{
		var options = fixture.CreateOptions(o =>
		{
			o.Username = null;
			o.Password = null;
			o.Token = sessionKey;
			o.MaxRetries = 0;
		});
		return new SplunkClient(options, new SessionKeyHandler(options.TrustedServerCertificateThumbprint));
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (request.Headers.Authorization is { Scheme: "Bearer", Parameter: { } key })
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Splunk", key);
		}

		return base.SendAsync(request, cancellationToken);
	}
}
