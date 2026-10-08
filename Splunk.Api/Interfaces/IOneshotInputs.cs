using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Files indexed once (<c>data/inputs/oneshot</c>). A oneshot input is listed only while it is in progress.</summary>
public interface IOneshotInputs
{
	/// <summary>Lists the oneshot inputs in progress (<c>GET data/inputs/oneshot</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per file being indexed.</returns>
	[Get("services/data/inputs/oneshot")]
	Task<SplunkFeed<OneshotInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Queues a file on the Splunk server for indexing, in full, even if it was indexed before
	/// (<c>POST data/inputs/oneshot</c>).
	/// </summary>
	/// <param name="request">The file and its metadata.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the queued file.</returns>
	[Post("services/data/inputs/oneshot")]
	Task<SplunkFeed<OneshotInput>> CreateAsync([Body] OneshotInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a oneshot input in progress (<c>GET data/inputs/oneshot/{name}</c>); 404 once it has finished.</summary>
	/// <param name="name">The file's path.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/oneshot/{name}")]
	Task<SplunkFeed<OneshotInput>> GetAsync(string name, CancellationToken cancellationToken);
}
