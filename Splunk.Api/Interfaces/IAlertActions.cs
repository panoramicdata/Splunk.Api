using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>Alert actions (<c>alerts/alert_actions</c>): the actions alerts can run and their defaults.</summary>
public interface IAlertActions
{
	/// <summary>Lists the alert actions (<c>GET alerts/alert_actions</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed of alert actions, named for example <c>email</c> or <c>webhook</c>.</returns>
	[Get("services/alerts/alert_actions")]
	Task<SplunkFeed<AlertAction>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);
}
