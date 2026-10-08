using Refit;

namespace Splunk.Api.Interfaces;

/// <summary>Server security operations (<c>server/security</c>).</summary>
public interface IServerSecurity
{
	/// <summary>Rotates the <c>splunk.secret</c> file of a standalone instance (<c>POST server/security/rotate-splunk-secret</c>).</summary>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when Splunk accepts the request.</returns>
	/// <remarks>
	/// Standalone Splunk Enterprise only. Every secret encrypted with the old key is re-encrypted; a restart is needed for
	/// the new secret to take full effect.
	/// </remarks>
	[Post("services/server/security/rotate-splunk-secret")]
	Task RotateSplunkSecretAsync(CancellationToken cancellationToken);
}
