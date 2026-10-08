using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Outputs;

namespace Splunk.Api.Interfaces;

/// <summary>Syslog forwarding groups (<c>data/outputs/tcp/syslog</c>).</summary>
public interface ISyslogOutputs
{
	/// <summary>Lists the syslog forwarding groups (<c>GET data/outputs/tcp/syslog</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per group.</returns>
	[Get("services/data/outputs/tcp/syslog")]
	Task<SplunkFeed<SyslogOutput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a syslog forwarding group (<c>POST data/outputs/tcp/syslog</c>).</summary>
	/// <param name="request">The group.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the group.</returns>
	[Post("services/data/outputs/tcp/syslog")]
	Task<SplunkFeed<SyslogOutput>> CreateAsync([Body] SyslogOutputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a syslog forwarding group (<c>GET data/outputs/tcp/syslog/{name}</c>).</summary>
	/// <param name="name">The group's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the group.</returns>
	[Get("services/data/outputs/tcp/syslog/{name}")]
	Task<SplunkFeed<SyslogOutput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a syslog forwarding group (<c>POST data/outputs/tcp/syslog/{name}</c>).</summary>
	/// <param name="name">The group's name.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the group.</returns>
	[Post("services/data/outputs/tcp/syslog/{name}")]
	Task<SplunkFeed<SyslogOutput>> UpdateAsync(string name, [Body] SyslogOutputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a syslog forwarding group (<c>DELETE data/outputs/tcp/syslog/{name}</c>).</summary>
	/// <param name="name">The group's name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the group is deleted.</returns>
	[Delete("services/data/outputs/tcp/syslog/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
