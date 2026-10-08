namespace Splunk.Api.Models.Knowledge;

/// <summary>
/// Reads and writes a wildcard form-field family, such as <c>alias.&lt;field&gt;</c> or
/// <c>lookup.field.input.&lt;column&gt;</c>, in a request's <see cref="SplunkFormRequest.AdditionalParameters"/>.
/// </summary>
internal static class PrefixedParameters
{
	/// <summary>The fields named <paramref name="prefix"/><c>{key}</c>, keyed by <c>{key}</c>.</summary>
	public static IReadOnlyDictionary<string, string> Get(IDictionary<string, string?> parameters, string prefix)
		=> parameters
			.Where(p => p.Key.StartsWith(prefix, StringComparison.Ordinal))
			.ToDictionary(p => p.Key[prefix.Length..], p => p.Value ?? string.Empty, StringComparer.Ordinal);

	/// <summary>Adds one field <paramref name="prefix"/><c>{key}</c> per entry of <paramref name="values"/>.</summary>
	public static void Set(IDictionary<string, string?> parameters, string prefix, IReadOnlyDictionary<string, string> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		foreach (var (key, value) in values)
		{
			parameters[prefix + key] = value;
		}
	}
}
