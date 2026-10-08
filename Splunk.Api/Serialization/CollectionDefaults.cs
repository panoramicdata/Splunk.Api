using System.Collections;
using System.Text.Json.Serialization.Metadata;

namespace Splunk.Api.Serialization;

/// <summary>
/// A <see cref="DefaultJsonTypeInfoResolver"/> modifier that makes a JSON <c>null</c> leave a collection property
/// (any list, array, set or dictionary, but not <see cref="string"/>) as its initialiser set it. Splunk sends
/// <c>"fields": null</c> where a list is empty, and models declare such lists as <c>IReadOnlyList&lt;T&gt; X = []</c>, so
/// callers can enumerate without a null check. A property with no initialiser still reads as <see langword="null"/>.
/// </summary>
internal static class CollectionDefaults
{
	public static void KeepOnNull(JsonTypeInfo typeInfo)
	{
		foreach (var property in typeInfo.Properties)
		{
			if (property.Set is { } set
				&& !property.IsExtensionData
				&& property.PropertyType != typeof(string)
				&& typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
			{
				property.Set = (target, value) =>
				{
					if (value is not null)
					{
						set(target, value);
					}
				};
			}
		}
	}
}
