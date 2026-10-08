using Refit;

namespace Splunk.Api.Models.Knowledge;

/// <summary>Options for reading one data model (<c>GET datamodel/model/{name}</c>).</summary>
public sealed class DataModelGetOptions
{
	/// <summary>When <see langword="true"/>, asks for the concise, human-readable description (<c>concise</c>).</summary>
	[AliasAs("concise")]
	public bool? Concise { get; init; }
}
