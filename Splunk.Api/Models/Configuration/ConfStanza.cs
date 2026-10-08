using System.Text.Json;

namespace Splunk.Api.Models.Configuration;

/// <summary>
/// A stanza of a <c>.conf</c> file (<c>configs/conf-{file}/{stanza}</c>). Its keys are free-form, so they are all in
/// <see cref="SplunkContent.AdditionalProperties"/>; <see cref="Values"/> and <see cref="GetValue(string)"/> read them as
/// text, the way they are written in the file.
/// </summary>
public sealed class ConfStanza : SplunkContent
{
	/// <summary>
	/// Every key of the stanza with its value as text: strings as given, numbers and booleans as their JSON text
	/// (<c>1</c>, <c>true</c>), <see langword="null"/> as <see langword="null"/>, and lists or objects as JSON.
	/// Splunk's own <c>eai:*</c> and <c>disabled</c> properties are not included.
	/// </summary>
	public IReadOnlyDictionary<string, string?> Values
		=> AdditionalProperties.ToDictionary(p => p.Key, p => AsText(p.Value), StringComparer.Ordinal);

	/// <summary>Gets one key's value as text (see <see cref="Values"/>).</summary>
	/// <param name="key">The key, as written in the file.</param>
	/// <returns>The value, or <see langword="null"/> when the stanza does not have the key or its value is null.</returns>
	public string? GetValue(string key)
		=> AdditionalProperties.TryGetValue(key, out var value) ? AsText(value) : null;

	private static string? AsText(JsonElement value)
		=> value.ValueKind switch
		{
			JsonValueKind.String => value.GetString(),
			JsonValueKind.Null or JsonValueKind.Undefined => null,
			_ => value.GetRawText()
		};
}
