using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Cluster;

/// <summary>The response of the search head cluster automated upgrade endpoints (<c>upgrade/shc/*</c>).</summary>
/// <remarks>These endpoints answer a <c>props</c> layout rather than the usual feed: entries have a <c>title</c> rather than a <c>name</c>, and their links are objects.</remarks>
/// <typeparam name="T">The type of each entry's content.</typeparam>
public sealed class ShClusterUpgradeResponse<T>
{
	/// <summary>When the response was generated, as Splunk formats it.</summary>
	[JsonPropertyName("updated")]
	public string? Updated { get; init; }

	/// <summary>The author, <c>Splunk</c>.</summary>
	[JsonPropertyName("author")]
	public string? Author { get; init; }

	/// <summary>The layout, <c>props</c>.</summary>
	[JsonPropertyName("layout")]
	public string? Layout { get; init; }

	/// <summary>The entries.</summary>
	[JsonPropertyName("entry")]
	public IReadOnlyList<ShClusterUpgradeEntry<T>> Entries { get; init; } = [];
}
