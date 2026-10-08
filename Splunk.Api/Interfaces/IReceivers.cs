using Refit;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Sends events to Splunk through the management port (<c>receivers/simple</c>, <c>receivers/stream</c>). Requires the
/// <c>edit_tcp</c> capability. For high volumes use the HTTP Event Collector (<see cref="SplunkHecClient"/>) instead.
/// </summary>
public interface IReceivers
{
	/// <summary>
	/// Indexes the request body as raw event text (<c>POST receivers/simple</c>); Splunk breaks it into events with
	/// the sourcetype's rules.
	/// </summary>
	/// <param name="events">The raw event text, sent as the whole body.</param>
	/// <param name="options">The events' host, index, source and sourcetype, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>What Splunk received and where the events went.</returns>
	[Post("services/receivers/simple")]
	Task<ReceiverResult> SendAsync([Body] string events, [Query] ReceiverOptions? options, CancellationToken cancellationToken);

	/// <summary>
	/// Indexes a stream of raw event text (<c>POST receivers/stream</c>), sent with
	/// <c>x-splunk-input-mode: streaming</c>. The stream is read to its end and is not retried. Requires the
	/// <c>edit_tcp</c> or <c>edit_tcp_stream</c> capability.
	/// </summary>
	/// <param name="events">The raw event text.</param>
	/// <param name="options">The events' host, index, source and sourcetype, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when Splunk has received the stream.</returns>
	[Post("services/receivers/stream")]
	[Headers("x-splunk-input-mode: streaming")]
	Task SendStreamAsync([Body] Stream events, [Query] ReceiverOptions? options, CancellationToken cancellationToken);
}
