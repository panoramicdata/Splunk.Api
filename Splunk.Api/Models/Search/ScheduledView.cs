using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Search;

/// <summary>
/// A dashboard scheduled for PDF delivery by email (<c>scheduled/views</c>). Its name is
/// <c>_ScheduledView__&lt;view name&gt;</c>. The email action's other settings (<c>action.email.*</c>) are in
/// <see cref="SplunkContent.AdditionalProperties"/>.
/// </summary>
public sealed class ScheduledView : ScheduledContent
{
	/// <summary>Whether the email action is enabled.</summary>
	[JsonPropertyName("action.email")]
	public bool ActionEmail { get; init; }

	/// <summary>The recipients, comma-separated.</summary>
	[JsonPropertyName("action.email.to")]
	public string? ActionEmailTo { get; init; }

	/// <summary>The view rendered to PDF.</summary>
	[JsonPropertyName("action.email.pdfview")]
	public string? ActionEmailPdfView { get; init; }

	/// <summary>The email subject for the view.</summary>
	[JsonPropertyName("action.email.subject.view")]
	public string? ActionEmailSubjectView { get; init; }
}
