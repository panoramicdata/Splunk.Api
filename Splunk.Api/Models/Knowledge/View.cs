using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Knowledge;

/// <summary>A view, usually a dashboard (<c>data/ui/views</c>).</summary>
public sealed class View : SplunkContent
{
	/// <summary>The view's source: Simple XML, or the Dashboard Studio definition (<c>eai:data</c>).</summary>
	[JsonPropertyName("eai:data")]
	public string? Data { get; init; }

	/// <summary>A hash of the current definition (<c>eai:digest</c>).</summary>
	[JsonPropertyName("eai:digest")]
	public string? Digest { get; init; }

	/// <summary>The user-interface type, <c>views</c> (<c>eai:type</c>).</summary>
	[JsonPropertyName("eai:type")]
	public string? Type { get; init; }

	/// <summary>The dashboard framework: 0 for Simple XML, 1 for Dashboard Studio, 2 for other views (<c>dashboardType</c>).</summary>
	[JsonPropertyName("dashboardType")]
	public int? DashboardType { get; init; }

	/// <summary>The view label.</summary>
	[JsonPropertyName("label")]
	public string? Label { get; init; }

	/// <summary>The view description.</summary>
	[JsonPropertyName("description")]
	public string? Description { get; init; }

	/// <summary>Whether the view is a dashboard (<c>isDashboard</c>).</summary>
	[JsonPropertyName("isDashboard")]
	public bool? IsDashboard { get; init; }

	/// <summary>Whether the view is shown in the app's navigation (<c>isVisible</c>).</summary>
	[JsonPropertyName("isVisible")]
	public bool? IsVisible { get; init; }

	/// <summary>The root XML element, for example <c>dashboard</c>, <c>form</c> or <c>view</c>.</summary>
	[JsonPropertyName("rootNode")]
	public string? RootNode { get; init; }

	/// <summary>The definition's schema version, for example <c>1.1</c>.</summary>
	[JsonPropertyName("version")]
	public string? Version { get; init; }

	/// <summary>The application suite the dashboard belongs to (<c>applicationSuite</c>).</summary>
	[JsonPropertyName("applicationSuite")]
	public string? ApplicationSuite { get; init; }

	/// <summary>Whether the dashboard can be embedded (<c>embed.enabled</c>).</summary>
	[JsonPropertyName("embed.enabled")]
	public bool? EmbedEnabled { get; init; }

	/// <summary>When an embedded dashboard expires, or 0 for never (<c>embed.expiry</c>).</summary>
	[JsonPropertyName("embed.expiry")]
	public long? EmbedExpiry { get; init; }
}
