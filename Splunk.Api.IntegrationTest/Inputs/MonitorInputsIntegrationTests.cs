using Splunk.Api.Models.Inputs;

namespace Splunk.Api.IntegrationTest.Inputs;

/// <summary>
/// A disabled monitor input on an existing Splunk-owned file (Splunk 10.6 refuses a path that does not exist), so
/// nothing is indexed; its sourcetype carries the test prefix.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class MonitorInputsIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task MonitorInput_RoundTrip()
	{
		var ct = TestContext.Current.CancellationToken;
		const string path = "$SPLUNK_HOME/etc/apps/search/default/app.conf";
		var sourcetype = SplunkFixture.UniqueName("monitor");
		var inputs = fixture.Client.MonitorInputs;
		try
		{
			var created = await inputs.CreateAsync(
				new MonitorInputCreateRequest { Name = path, Disabled = true, Sourcetype = sourcetype, CrcSalt = "<SOURCE>", IgnoreOlderThan = "1d" },
				ct);
			var input = created.Entries.Should().ContainSingle().Subject;
			input.Name.Should().Be(path);
			input.Content!.CrcSalt.Should().Be("<SOURCE>");

			await inputs.UpdateAsync(path, new MonitorInputUpdateRequest { Whitelist = "\\.log$", Disabled = true }, ct);

			var read = (await inputs.GetAsync(path, ct)).Entries.Single().Content!;
			read.Whitelist.Should().Be("\\.log$");
			read.IgnoreOlderThan.Should().Be("1d");
			read.Disabled.Should().BeTrue();
			read.Sourcetype.Should().Be(sourcetype);
			(await inputs.ListMembersAsync(path, null, ct)).Entries.Should().BeEmpty("the input is disabled");
		}
		finally
		{
			await inputs.DeleteAsync(path, ct);
		}

		var act = () => inputs.GetAsync(path, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
	}
}
