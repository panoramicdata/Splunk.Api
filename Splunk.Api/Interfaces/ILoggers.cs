using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Server;

namespace Splunk.Api.Interfaces;

/// <summary>splunkd logging categories and their levels (<c>server/logger</c>).</summary>
public interface ILoggers
{
	/// <summary>Lists the logging categories (<c>GET server/logger</c>). There are nearly 2,000; page through them.</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per category.</returns>
	[Get("services/server/logger")]
	Task<SplunkFeed<LoggerCategory>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets one logging category (<c>GET server/logger/{name}</c>).</summary>
	/// <param name="name">The category, for example <c>TcpOutputProc</c>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the category.</returns>
	[Get("services/server/logger/{name}")]
	Task<SplunkFeed<LoggerCategory>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a category's level until splunkd restarts (<c>POST server/logger/{name}</c>).</summary>
	/// <param name="name">The category.</param>
	/// <param name="request">The new level.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated category.</returns>
	[Post("services/server/logger/{name}")]
	Task<SplunkFeed<LoggerCategory>> UpdateAsync(string name, [Body] LoggerUpdateRequest request, CancellationToken cancellationToken);
}
