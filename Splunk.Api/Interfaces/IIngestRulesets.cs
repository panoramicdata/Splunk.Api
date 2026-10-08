using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Ingest actions rulesets (<c>data/ingest/rulesets</c>): filtering, masking and routing applied at ingest time.</summary>
public interface IIngestRulesets
{
	/// <summary>Lists the rulesets (<c>GET data/ingest/rulesets</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per ruleset (Splunk sends no paging details).</returns>
	[Get("services/data/ingest/rulesets")]
	Task<SplunkFeed<IngestRuleset>> ListAsync(CancellationToken cancellationToken);

	/// <summary>Creates a ruleset (<c>POST data/ingest/rulesets</c>).</summary>
	/// <param name="request">The ruleset.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the ruleset.</returns>
	[Post("services/data/ingest/rulesets")]
	Task<SplunkFeed<IngestRuleset>> CreateAsync([Body] IngestRulesetCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a ruleset (<c>GET data/ingest/rulesets/{name}</c>).</summary>
	/// <param name="name">The ruleset's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the ruleset.</returns>
	[Get("services/data/ingest/rulesets/{name}")]
	Task<SplunkFeed<IngestRuleset>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a ruleset (<c>POST data/ingest/rulesets/{name}</c>).</summary>
	/// <remarks>
	/// The reference lists <c>Match</c> and <c>Action</c> parameters; Splunk 10.6 takes the same <c>sourcetype</c>,
	/// <c>description</c> and <c>rules</c> fields as creation, and finds the ruleset by name and sourcetype.
	/// </remarks>
	/// <param name="name">The ruleset's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the ruleset.</returns>
	[Post("services/data/ingest/rulesets/{name}")]
	Task<SplunkFeed<IngestRuleset>> UpdateAsync(string name, [Body] IngestRulesetUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>
	/// Pushes ruleset changes to the indexer cluster (<c>POST data/ingest/rulesets/publish</c>). Only a cluster manager
	/// accepts it; any other node answers 400.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the published rulesets.</returns>
	[Post("services/data/ingest/rulesets/publish")]
	Task<SplunkFeed<IngestRuleset>> PublishAsync(CancellationToken cancellationToken);
}
