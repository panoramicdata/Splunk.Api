using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ServerSecurityTests
{
	[Fact]
	public async Task RotateSplunkSecretAsync_PostsWithNoBody()
		=> await Calls.AssertAsync(
			c => c.ServerSecurity.RotateSplunkSecretAsync(Calls.Token),
			HttpMethod.Post, "/services/server/security/rotate-splunk-secret", Calls.JsonQuery, null);

	[Fact]
	public async Task RotateSplunkSecretAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.ServerSecurity.RotateSplunkSecretAsync(Calls.Token), HttpStatusCode.BadRequest);
}
