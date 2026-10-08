using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Splunk.Api.Serialization;

/// <summary>
/// The wire names of enum members: the <see cref="JsonStringEnumMemberNameAttribute"/> name where present, otherwise the
/// member name. Shared by JSON, form and query formatting so every channel spells a value the same way.
/// </summary>
internal static class WireNames
{
	private static readonly ConcurrentDictionary<Type, EnumNames> Cache = new();

	/// <summary>The wire name of an enum value; an undefined value is written as its number.</summary>
	public static string Of(Enum value)
	{
		var names = Cache.GetOrAdd(value.GetType(), Build);
		return names.ByValue.TryGetValue(value, out var name) ? name : Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture).ToString(System.Globalization.CultureInfo.InvariantCulture);
	}

	/// <summary>Parses a wire name case-insensitively, returning the default value for an unrecognised name.</summary>
	public static T Parse<T>(string wireName) where T : struct, Enum
	{
		var names = Cache.GetOrAdd(typeof(T), Build);
		return names.ByName.TryGetValue(wireName, out var value) ? (T)value : default;
	}

	private static EnumNames Build(Type enumType)
	{
		var byName = new Dictionary<string, Enum>(StringComparer.OrdinalIgnoreCase);
		var byValue = new Dictionary<Enum, string>();
		foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
		{
			var value = (Enum)field.GetValue(null)!;
			var name = field.GetCustomAttribute<JsonStringEnumMemberNameAttribute>()?.Name ?? field.Name;
			byName[name] = value;
			byValue.TryAdd(value, name);
		}

		return new EnumNames(byName, byValue);
	}

	private sealed record EnumNames(Dictionary<string, Enum> ByName, Dictionary<Enum, string> ByValue);
}
