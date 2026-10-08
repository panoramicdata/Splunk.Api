using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>The machine's resources and operating system settings (<c>server/sysinfo</c>).</summary>
public interface ISystemInfo
{
	/// <summary>Gets the machine's resources and operating system settings (<c>GET server/sysinfo</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, <c>system-info</c>.</returns>
	[Get("services/server/sysinfo")]
	Task<SplunkFeed<SystemInfo>> GetAsync(CancellationToken cancellationToken);
}
