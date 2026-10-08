using Refit;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Options for listing data models (<c>GET datamodel/model</c>).</summary>
public sealed class DataModelListOptions : ListOptions
{
	/// <summary>When <see langword="true"/>, asks for the concise, human-readable description of each model (<c>concise</c>).</summary>
	[AliasAs("concise")]
	public bool? Concise { get; init; }
}
