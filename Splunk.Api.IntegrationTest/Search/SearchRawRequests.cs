using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace Splunk.Api.IntegrationTest.Search;

/// <summary>
/// Raw form requests for test set-up the search interfaces do not cover (creating a dashboard to schedule, or a metric
/// index to catalogue), authenticated as <see cref="SplunkFixture"/> is configured.
/// </summary>
internal static class SearchRawRequests
{
	public static async Task SendAsync(SplunkFixture fixture, HttpMethod method, string path, IDictionary<string, string>? form)
	{
		// The shared instance occasionally resets a connection during the TLS handshake, before the request is sent.
		for (var attempt = 1; ; attempt++)
		{
			try
			{
				await SendOnceAsync(fixture, method, path, form);
				return;
			}
			catch (HttpRequestException exception) when (exception.StatusCode is null && attempt < 4)
			{
				await Task.Delay(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
			}
		}
	}

	private static async Task SendOnceAsync(SplunkFixture fixture, HttpMethod method, string path, IDictionary<string, string>? form)
	{
		var options = fixture.CreateOptions();
		using var handler = new HttpClientHandler();
		var pinned = options.TrustedServerCertificateThumbprint?.Replace(":", string.Empty, StringComparison.Ordinal);
		if (!string.IsNullOrEmpty(pinned))
		{
			handler.ServerCertificateCustomValidationCallback = (_, certificate, _, errors)
				=> errors == System.Net.Security.SslPolicyErrors.None
					|| string.Equals(certificate?.GetCertHashString(HashAlgorithmName.SHA256), pinned, StringComparison.OrdinalIgnoreCase);
		}

		using var http = new HttpClient(handler) { BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/") };
		using var request = new HttpRequestMessage(method, path + "?output_mode=json");
		request.Headers.Authorization = string.IsNullOrEmpty(options.Token)
			? new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{options.Username}:{options.Password}")))
			: new AuthenticationHeaderValue("Bearer", options.Token);
		if (form is not null)
		{
			request.Content = new FormUrlEncodedContent(form);
		}

		using var response = await http.SendAsync(request, TestContext.Current.CancellationToken);
		response.EnsureSuccessStatusCode();
	}
}
