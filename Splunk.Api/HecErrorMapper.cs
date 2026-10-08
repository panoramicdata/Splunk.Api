using Splunk.Api.Models.Inputs;
using System.Text.Json;

namespace Splunk.Api;

/// <summary>Turns a non-success HTTP Event Collector response into a <see cref="SplunkHecException"/>.</summary>
internal static class HecErrorMapper
{
	public static async Task<Exception?> CreateAsync(HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			return null;
		}

		var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
		var reply = Parse(body);
		var text = string.IsNullOrEmpty(reply?.Text)
			? $"HTTP {(int)response.StatusCode} ({response.ReasonPhrase ?? response.StatusCode.ToString()})"
			: reply.Text;
		return new SplunkHecException(response.StatusCode, reply?.Code, reply?.InvalidEventNumber, text);
	}

	private static HecResponse? Parse(string body)
	{
		try
		{
			return JsonSerializer.Deserialize<HecResponse>(body, SplunkJson.Options);
		}
		catch (JsonException)
		{
			return null;
		}
	}
}
