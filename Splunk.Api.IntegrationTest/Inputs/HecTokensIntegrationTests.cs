using Splunk.Api.Models.Inputs;

namespace Splunk.Api.IntegrationTest.Inputs;

/// <summary>An HTTP Event Collector token round trip; the shared instance's collector stays as it is.</summary>
[Collection(SplunkTestGroup.Name)]
public class HecTokensIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task HecToken_RoundTrip()
	{
		var ct = TestContext.Current.CancellationToken;
		var name = SplunkFixture.UniqueName("hec");
		var tokens = fixture.Client.HecTokens;
		try
		{
			var created = (await tokens.CreateAsync(
				new HecTokenCreateRequest { Name = name, Index = "main", Indexes = ["main"], Sourcetype = "splunk_api_it", UseAck = true, Description = "Splunk.Api test" },
				ct)).Entries.Should().ContainSingle().Subject;
			created.Name.Should().Be("http://" + name);
			var value = created.Content!.Token;
			Guid.TryParse(value, out _).Should().BeTrue();
			created.Content.UseAck.Should().BeTrue();

			(await tokens.UpdateAsync(name, new HecTokenUpdateRequest { Sourcetype = "splunk_api_it_v2" }, ct)).Entries.Single().Content!.Sourcetype.Should().Be("splunk_api_it_v2");
			(await tokens.DisableAsync(name, ct)).Entries.Single().Content!.Disabled.Should().BeTrue();
			(await tokens.EnableAsync(name, ct)).Entries.Single().Content!.Disabled.Should().BeFalse();
			(await tokens.RotateAsync(name, ct)).Entries.Single().Content!.Token.Should().NotBe(value);

			var read = (await tokens.GetAsync(name, ct)).Entries.Single().Content!;
			read.Indexes.Should().Equal("main");
			read.Description.Should().Be("Splunk.Api test");
			(await tokens.ListAsync(new Models.ListOptions { Count = 0 }, ct)).Entries.Should().Contain(e => e.Name == "http://" + name);
		}
		finally
		{
			await tokens.DeleteAsync(name, ct);
		}

		var act = () => tokens.GetAsync(name, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
	}
}
