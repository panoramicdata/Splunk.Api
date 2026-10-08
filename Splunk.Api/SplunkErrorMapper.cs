using Splunk.Api.Models;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace Splunk.Api;

/// <summary>
/// Turns a non-success response into a <see cref="SplunkApiException"/> carrying Splunk's messages. Splunk answers errors
/// with <c>{"messages":[{"type":"ERROR","text":"..."}]}</c> in JSON mode, and with
/// <c>&lt;response&gt;&lt;messages&gt;&lt;msg type="ERROR"&gt;...&lt;/msg&gt;</c> where it ignores the output mode.
/// </summary>
internal static class SplunkErrorMapper
{
	public static async Task<Exception?> CreateAsync(HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			return null;
		}

		var body = await ReadBodyAsync(response).ConfigureAwait(false);
		var messages = ParseMessages(body);
		var text = PrimaryText(messages)
			?? $"HTTP {(int)response.StatusCode} ({response.ReasonPhrase ?? response.StatusCode.ToString()})";
		return new SplunkApiException(response.StatusCode, messages, text);
	}

	/// <summary>The first <c>ERROR</c> message's text, else the first message's text, else <see langword="null"/>.</summary>
	private static string? PrimaryText(IReadOnlyList<SplunkMessage> messages)
	{
		foreach (var message in messages)
		{
			if (message.Type.Equals("ERROR", StringComparison.OrdinalIgnoreCase) && message.Text.Length > 0)
			{
				return message.Text;
			}
		}

		return messages.Count > 0 && messages[0].Text.Length > 0 ? messages[0].Text : null;
	}

	internal static IReadOnlyList<SplunkMessage> ParseMessages(string body)
	{
		var trimmed = body.TrimStart();
		if (trimmed.StartsWith('{'))
		{
			return ParseJson(trimmed);
		}

		return trimmed.StartsWith('<') ? ParseXml(trimmed) : [];
	}

	private static List<SplunkMessage> ParseJson(string body)
	{
		try
		{
			using var document = JsonDocument.Parse(body);
			if (!document.RootElement.TryGetProperty("messages", out var messages) || messages.ValueKind != JsonValueKind.Array)
			{
				return [];
			}

			return [.. messages.EnumerateArray()
				.Where(m => m.ValueKind == JsonValueKind.Object)
				.Select(m => new SplunkMessage { Type = ReadString(m, "type"), Text = ReadString(m, "text") })];
		}
		catch (JsonException)
		{
			return [];
		}
	}

	private static List<SplunkMessage> ParseXml(string body)
	{
		try
		{
			var document = XDocument.Parse(body);
			return [.. document.Descendants("msg")
				.Select(m => new SplunkMessage { Type = (string?)m.Attribute("type") ?? string.Empty, Text = m.Value.Trim() })];
		}
		catch (XmlException)
		{
			return [];
		}
	}

	private static string ReadString(JsonElement element, string name)
		=> element.TryGetProperty(name, out var property) && property.ValueKind == JsonValueKind.String
			? property.GetString() ?? string.Empty
			: string.Empty;

	private static async Task<string> ReadBodyAsync(HttpResponseMessage response)
	{
		try
		{
			return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
		}
		catch (InvalidOperationException)
		{
			// Unsupported charset in Content-Type: the body is unreadable, use the fallback message.
			return string.Empty;
		}
	}
}
