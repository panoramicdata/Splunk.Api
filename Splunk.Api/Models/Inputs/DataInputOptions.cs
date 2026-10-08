using Refit;

namespace Splunk.Api.Models.Inputs;

/// <summary>Options for reading one input from <c>data/inputs/all/{name}</c>.</summary>
public sealed class DataInputOptions
{
	/// <summary>When <see langword="true"/>, only the properties common to every input are returned (<c>common</c>).</summary>
	[AliasAs("common")]
	public bool? Common { get; init; }
}
