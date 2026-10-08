using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Knowledge;

namespace Splunk.Api.Interfaces;

/// <summary>Dimension extraction from StatsD metrics (<c>data/transforms/statsdextractions</c>).</summary>
/// <remarks>Requires the <c>edit_statsd_transforms</c> capability. The reference documents only creation.</remarks>
public interface IStatsdExtractions
{
	/// <summary>Creates a StatsD dimension extraction (<c>POST data/transforms/statsdextractions</c>).</summary>
	/// <param name="request">The name and regular expression.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the new extraction, named <c>statsd-dims:{name}</c>.</returns>
	[Post("services/data/transforms/statsdextractions")]
	Task<SplunkFeed<StatsdExtraction>> CreateAsync([Body] StatsdExtractionCreateRequest request, CancellationToken cancellationToken);
}
