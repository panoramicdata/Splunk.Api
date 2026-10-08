using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Ingest actions S3 destinations (<c>data/ingest/rfsdestinations</c>). Requires the <c>list_ingest_rulesets</c> and
/// <c>edit_ingest_rulesets</c> capabilities.
/// </summary>
public interface IIngestDestinations
{
	/// <summary>Lists the S3 destinations (<c>GET data/ingest/rfsdestinations</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per destination.</returns>
	[Get("services/data/ingest/rfsdestinations")]
	Task<SplunkFeed<IngestDestination>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Creates an S3 destination, or replaces the settings of the one with the same name
	/// (<c>POST data/ingest/rfsdestinations</c>). Splunk does not test the bucket or the credentials.
	/// </summary>
	/// <param name="request">The destination.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the destination.</returns>
	[Post("services/data/ingest/rfsdestinations")]
	Task<SplunkFeed<IngestDestination>> CreateAsync([Body] IngestDestinationCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes an S3 destination (<c>DELETE data/ingest/rfsdestinations/{name}</c>).</summary>
	/// <remarks>
	/// The reference documents <c>DELETE data/ingest/rfsdestinations</c> with a <c>name</c> parameter, but Splunk 10.6
	/// answers that with "Cannot perform action DELETE without a target name"; the name must be in the path.
	/// </remarks>
	/// <param name="name">The destination's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the destination is deleted.</returns>
	[Delete("services/data/ingest/rfsdestinations/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
