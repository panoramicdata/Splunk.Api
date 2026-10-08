using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;
using System.Net;

namespace Splunk.Api.IntegrationTest.Introspection;

[Collection(SplunkTestGroup.Name)]
public class IndexesIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Token => TestContext.Current.CancellationToken;

	[Fact]
	public async Task Indexes_ListAndGetMain()
	{
		var all = await fixture.Client.Indexes.ListAsync(new IndexListOptions { DataType = IndexDataType.All, Count = 0 }, Token);
		var main = await fixture.Client.Indexes.GetAsync("main", new IndexGetOptions { Summarize = true }, Token);
		var extended = await fixture.Client.IndexesExtended.GetAsync("main", Token);
		var extendedList = await fixture.Client.IndexesExtended.ListAsync(new IndexListOptions { Count = 2 }, Token);

		all.Entries.Select(e => e.Name).Should().Contain("main").And.Contain("_internal");
		main.Entries.Should().ContainSingle().Which.Content!.DataType.Should().Be(IndexDataType.Event);
		extended.Entries.Should().ContainSingle().Which.Content!.TotalSizeMB.Should().NotBeNull();
		extendedList.Entries.Should().HaveCount(2);
	}

	[Fact]
	public async Task Index_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("idx").ToLowerInvariant();
		var app = fixture.Client.InNamespace("nobody", "search");
		var created = await app.Indexes.CreateAsync(new IndexCreateRequest { Name = name, DataType = IndexDataType.Event, MaxTotalDataSizeMB = 100 }, Token);
		try
		{
			created.Entries.Should().ContainSingle().Which.Content!.MaxTotalDataSizeMB.Should().Be(100);

			var updated = await app.Indexes.UpdateAsync(name, new IndexUpdateRequest { FrozenTimePeriodInSecs = 86400 }, Token);
			updated.Entries.Should().ContainSingle().Which.Content!.FrozenTimePeriodInSecs.Should().Be(86400);
		}
		finally
		{
			await app.Indexes.DeleteAsync(name, CancellationToken.None);
		}

		var act = () => fixture.Client.Indexes.GetAsync(name, null, Token);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task VolumesAndSummaries_AreRead()
	{
		var volumes = await fixture.Client.IndexVolumes.ListAsync(null, Token);
		var volume = await fixture.Client.IndexVolumes.GetAsync(volumes.Entries[0].Name, Token);
		var summaries = await fixture.Client.DataSummaries.ListAsync(new SummaryListOptions { ReportAcceleration = true, DataModelAcceleration = true }, Token);

		volume.Entries.Should().ContainSingle().Which.Content!.MaxSize.Should().NotBeNullOrEmpty();
		summaries.Paging.Should().NotBeNull();
	}

	[Fact]
	public async Task MissingSummary_IsAnError()
	{
		var act = () => fixture.Client.DataSummaries.GetAsync(SplunkFixture.UniqueName("summary"), Token);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().NotBeNullOrWhiteSpace();
	}
}
