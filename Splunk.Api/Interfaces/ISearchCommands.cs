using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>Custom (Python) search commands (<c>data/commands</c>). Built-in commands such as <c>eval</c> are not listed.</summary>
public interface ISearchCommands
{
	/// <summary>Lists the custom search commands (<c>GET data/commands</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed of commands.</returns>
	[Get("services/data/commands")]
	Task<SplunkFeed<SearchCommand>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a custom search command (<c>GET data/commands/{name}</c>).</summary>
	/// <param name="name">The command name, for example <c>bucketdir</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/data/commands/{name}")]
	Task<SplunkFeed<SearchCommand>> GetAsync(string name, CancellationToken cancellationToken);
}
