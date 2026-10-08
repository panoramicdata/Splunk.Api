using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Deployment;

/// <summary>Renames a server class (<c>POST deployment/server/serverclasses/rename</c>).</summary>
public sealed class DeploymentServerClassRenameRequest : SplunkFormRequest
{
	/// <summary>The current name.</summary>
	[JsonPropertyName("oldName")]
	public required string OldName { get; init; }

	/// <summary>The new name.</summary>
	[JsonPropertyName("newName")]
	public required string NewName { get; init; }
}
