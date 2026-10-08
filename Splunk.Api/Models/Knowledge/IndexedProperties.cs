using System.Text.Json;

namespace Splunk.Api.Models.Knowledge;

/// <summary>
/// Reads Splunk's flattened property families such as <c>alias.0.src</c> or <c>lookup.field.output.1.owner</c> into
/// name/value pairs.
/// </summary>
internal static class IndexedProperties
{
	/// <summary>
	/// Collects every property whose name starts with <paramref name="prefix"/>, in the order Splunk returned them, keyed
	/// by the rest of the name with any leading position number (<c>0.</c>, <c>1.</c>...) removed. A key can repeat (one
	/// field with two aliases), so the result is a list rather than a dictionary.
	/// </summary>
	public static IReadOnlyList<KeyValuePair<string, string>> Collect(IDictionary<string, JsonElement> properties, string prefix)
		=> [.. properties
			.Where(p => p.Key.StartsWith(prefix, StringComparison.Ordinal))
			.Select(p => new KeyValuePair<string, string>(
				StripPosition(p.Key[prefix.Length..]),
				p.Value.ValueKind == JsonValueKind.String ? p.Value.GetString()! : p.Value.GetRawText()))];

	private static string StripPosition(string key)
	{
		var dot = key.IndexOf('.', StringComparison.Ordinal);
		return dot > 0 && key[..dot].All(char.IsAsciiDigit) ? key[(dot + 1)..] : key;
	}
}
