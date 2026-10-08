using Refit;

namespace Splunk.Api.Models.Inputs;

/// <summary>Options for listing <c>data/inputs/all</c>.</summary>
public sealed class DataInputListOptions : ListOptions
{
	/// <summary>When <see langword="true"/>, only the properties common to every input are returned (<c>common</c>).</summary>
	[AliasAs("common")]
	public bool? Common { get; init; }
}
