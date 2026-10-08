using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Splunk.Api.IntegrationTest.Knowledge;

/// <summary>Removes the objects the knowledge integration tests create.</summary>
internal static class Cleanup
{
	/// <summary>Runs a delete, ignoring 404 so a failed create does not hide the original failure.</summary>
	public static async Task IgnoreMissingAsync(Func<Task> delete)
	{
		try
		{
			await delete();
		}
		catch (SplunkApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
		{
			// Already gone, or never created.
		}
	}

	/// <summary>
	/// Deletes a test object in <c>nobody/search</c> through an endpoint the 10.6 reference does not document, and so the
	/// client does not offer: a panel (<c>data/ui/panels/{name}</c>) or a StatsD extraction
	/// (<c>data/transforms/statsdextractions/{name}</c>). Only names with <see cref="SplunkFixture.Prefix"/> are accepted.
	/// </summary>
	public static async Task DeleteUndocumentedAsync(SplunkFixture fixture, string servicesPath, string name)
	{
		if (!name.StartsWith(SplunkFixture.Prefix, StringComparison.Ordinal))
		{
			throw new ArgumentException("Only test objects may be deleted.", nameof(name));
		}

		var options = fixture.CreateOptions();
		var thumbprint = options.TrustedServerCertificateThumbprint?.Replace(":", string.Empty, StringComparison.Ordinal);
		using var handler = new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback = (_, certificate, _, errors)
				=> errors == System.Net.Security.SslPolicyErrors.None
					|| (certificate is not null && string.Equals(certificate.GetCertHashString(HashAlgorithmName.SHA256), thumbprint, StringComparison.OrdinalIgnoreCase))
		};
		using var http = new HttpClient(handler) { BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/") };
		http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(options.Token)
			? new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.Username}:{options.Password}")))
			: new AuthenticationHeaderValue("Bearer", options.Token);

		using var response = await http.DeleteAsync($"servicesNS/nobody/search/{servicesPath}/{Uri.EscapeDataString(name)}?output_mode=json", CancellationToken.None);
		response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
	}

	/// <summary>Asserts that reading an object now answers 404.</summary>
	public static async Task AssertGoneAsync(Func<Task> get)
		=> (await get.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
