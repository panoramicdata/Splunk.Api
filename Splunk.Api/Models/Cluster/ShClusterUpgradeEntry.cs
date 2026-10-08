using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>One entry of a search head cluster automated upgrade response.</summary>
/// <typeparam name="T">The type of <see cref="Content"/>.</typeparam>
public sealed class ShClusterUpgradeEntry<T>
{
	/// <summary>The entry title, for example <c>status</c>.</summary>
	[JsonPropertyName("title")]
	public string? Title { get; init; }

	/// <summary>The entry path.</summary>
	[JsonPropertyName("id")]
	public string? Id { get; init; }

	/// <summary>When the entry was generated, as Splunk formats it (for example <c>2022-11-24T17:36:20+0000</c>).</summary>
	[JsonPropertyName("updated")]
	public string? Updated { get; init; }

	/// <summary>The entry content.</summary>
	[JsonPropertyName("content")]
	public T? Content { get; init; }
}
