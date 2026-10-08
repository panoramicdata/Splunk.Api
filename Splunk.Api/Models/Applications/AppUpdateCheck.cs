using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Applications;

/// <summary>
/// The result of checking Splunkbase for an update to an app (<c>apps/local/{name}/update</c>). The <c>update.*</c>
/// properties are present only when Splunkbase has a newer version; otherwise the content carries just <c>eai:acl</c>.
/// </summary>
public sealed class AppUpdateCheck : SplunkContent
{
	/// <summary>The name of the available update (<c>update.name</c>).</summary>
	[JsonPropertyName("update.name")]
	public string? UpdateName { get; init; }

	/// <summary>The version of the available update (<c>update.version</c>).</summary>
	[JsonPropertyName("update.version")]
	public string? UpdateVersion { get; init; }

	/// <summary>The Splunkbase page of the update (<c>update.homepage</c>).</summary>
	[JsonPropertyName("update.homepage")]
	public string? UpdateHomepage { get; init; }

	/// <summary>The download URL of the update (<c>update.appurl</c>).</summary>
	[JsonPropertyName("update.appurl")]
	public string? UpdateAppUrl { get; init; }

	/// <summary>The size of the update package, in bytes (<c>update.size</c>).</summary>
	[JsonPropertyName("update.size")]
	public long? UpdateSize { get; init; }

	/// <summary>The checksum of the update package (<c>update.checksum</c>).</summary>
	[JsonPropertyName("update.checksum")]
	public string? UpdateChecksum { get; init; }

	/// <summary>The checksum algorithm, for example <c>md5</c> (<c>update.checksum.type</c>).</summary>
	[JsonPropertyName("update.checksum.type")]
	public string? UpdateChecksumType { get; init; }

	/// <summary>Whether installing the update needs the app's ID given explicitly (<c>update.implicit_id_required</c>).</summary>
	[JsonPropertyName("update.implicit_id_required")]
	public bool? UpdateImplicitIdRequired { get; init; }
}
