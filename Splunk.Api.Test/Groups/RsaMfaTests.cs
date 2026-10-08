using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class RsaMfaTests
{
	// Shaped after the reference (a live standalone instance has no RSA configuration).
	private const string RsaJson = """
		{
			"entry": [
				{
					"name": "rsa-mfa",
					"content": {
						"authManagerUrl": "https://rsa.example.com:5555/mfa/v1_1",
						"accessKey": "********",
						"clientId": "splunk-agent",
						"failOpen": "1",
						"timeout": "5",
						"messageOnError": "Contact the help desk",
						"enableMfaAuthRest": "0",
						"caCertBundlePayload": "-----BEGIN CERTIFICATE-----",
						"replicateCertificates": "1"
					}
				}
			]
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestAssert.SendAsync((c, ct) => c.RsaMfa.ListAsync(null, ct)))
			.ShouldBe(HttpMethod.Get, "/services/admin/Rsa-MFA");

	[Fact]
	public async Task SaveAsync_PostsTheConfiguration()
		=> (await RequestAssert.SendAsync((c, ct) => c.RsaMfa.SaveAsync(
			new RsaMfaRequest
			{
				Name = "rsa",
				AuthManagerUrl = "https://rsa",
				AccessKey = "key",
				ClientId = "agent",
				CaCertBundlePayload = "pem",
				FailOpen = true,
				Timeout = 5,
				MessageOnError = "no",
				EnableMfaAuthRest = false,
				ReplicateCertificates = true
			},
			ct)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/admin/Rsa-MFA",
				"name=rsa&authManagerUrl=https%3A%2F%2Frsa&accessKey=key&clientId=agent&caCertBundlePayload=pem&failOpen=true&timeout=5&messageOnError=no&enableMfaAuthRest=false&replicateCertificates=true");

	[Fact]
	public async Task DeleteAsync_SendsDeleteToTheCollection()
		=> (await RequestAssert.SendAsync((c, ct) => c.RsaMfa.DeleteAsync(ct)))
			.ShouldBe(HttpMethod.Delete, "/services/admin/Rsa-MFA");

	[Fact]
	public async Task VerifyAsync_PostsTheUserAndPasscode()
		=> (await RequestAssert.SendAsync((c, ct) => c.RsaMfa.VerifyAsync("rsa-mfa", new RsaMfaVerifyRequest { Username = "jo", Passcode = "1234567890" }, ct)))
			.ShouldBe(HttpMethod.Post, "/services/admin/Rsa-MFA-config-verify/rsa-mfa", "username=jo&passcode=1234567890");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await RequestAssert.ReadAsync((c, ct) => c.RsaMfa.ListAsync(null, ct), RsaJson);

		var rsa = feed.Entries.Should().ContainSingle().Subject.Content!;
		rsa.AuthManagerUrl.Should().Be("https://rsa.example.com:5555/mfa/v1_1");
		rsa.AccessKey.Should().Be("********");
		rsa.ClientId.Should().Be("splunk-agent");
		rsa.FailOpen.Should().BeTrue();
		rsa.Timeout.Should().Be(5);
		rsa.MessageOnError.Should().Be("Contact the help desk");
		rsa.EnableMfaAuthRest.Should().BeFalse();
		rsa.CaCertBundlePayload.Should().Be("-----BEGIN CERTIFICATE-----");
		rsa.ReplicateCertificates.Should().BeTrue();
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.RsaMfa.VerifyAsync("missing", new RsaMfaVerifyRequest(), ct));
}
