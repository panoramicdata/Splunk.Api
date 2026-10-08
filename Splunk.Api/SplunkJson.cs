using Splunk.Api.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api;

/// <summary>Shared <see cref="JsonSerializerOptions"/> for Splunk payloads.</summary>
/// <remarks>
/// Splunk is loose about JSON types: the same property can arrive as <c>true</c>, <c>1</c>, <c>"1"</c> or <c>"true"</c>,
/// and numbers often arrive as strings, sometimes empty. These options read all of those forms, map unrecognised enum
/// names to the enum's default (<c>Unknown</c>) value, and read numbers and booleans into string properties as their text.
/// A JSON <c>null</c> for a collection property keeps the property's initial value (typically <c>[]</c>).
/// </remarks>
public static class SplunkJson
{
	/// <summary>
	/// The options used by <see cref="SplunkClient"/>. They are read-only: copy them
	/// (<c>new JsonSerializerOptions(SplunkJson.Options)</c>) to customise.
	/// </summary>
	public static JsonSerializerOptions Options { get; } = Create();

	private static JsonSerializerOptions Create()
	{
		var options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
			TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver
			{
				Modifiers = { CollectionDefaults.KeepOnNull }
			},
		};
		options.Converters.Add(new TolerantBooleanConverter());
		options.Converters.Add(new TolerantNullableBooleanConverter());
		options.Converters.Add(new TolerantStringConverter());
		options.Converters.Add(new TolerantInt32Converter());
		options.Converters.Add(new TolerantNullableInt32Converter());
		options.Converters.Add(new TolerantInt64Converter());
		options.Converters.Add(new TolerantNullableInt64Converter());
		options.Converters.Add(new TolerantDoubleConverter());
		options.Converters.Add(new TolerantNullableDoubleConverter());
		options.Converters.Add(new TolerantEnumConverterFactory());
		// Shared and static: lock it so no caller can change serialization for every client.
		options.MakeReadOnly();
		return options;
	}
}
