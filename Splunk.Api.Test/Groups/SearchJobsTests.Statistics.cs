using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class SearchJobsTests
{
	[Fact]
	public async Task GetSummaryAsync_SendsGetWithEveryOption()
	{
		var stub = TestClient.Stub(SearchJson.Summary);
		using var client = TestClient.Create(stub);

		await client.SearchJobs.GetSummaryAsync(
			"my_sid",
			new SearchJobSummaryOptions
			{
				Fields = ["host", "linecount"],
				EarliestTime = "-1h",
				LatestTime = "now",
				Search = "error",
				MinFrequency = 0.5,
				TopCount = 3,
				Histogram = true,
				OutputTimeFormat = "%s",
				TimeFormat = "%s"
			},
			Ct);

		stub.ShouldHaveSent(
			HttpMethod.Get,
			"/services/search/jobs/my_sid/summary",
			"?f=host&f=linecount&earliest_time=-1h&latest_time=now&search=error&min_freq=0.5&top_count=3&histogram=true"
				+ "&output_time_format=%25s&time_format=%25s&output_mode=json");
	}

	[Fact]
	public async Task GetSummaryAsync_MapsTheFieldStatistics()
	{
		using var client = TestClient.Create(TestClient.Stub(SearchJson.Summary));

		var summary = (await client.SearchJobs.GetSummaryAsync("my_sid", null, Ct))!;

		summary.EarliestTime.Should().Be(new DateTimeOffset(2026, 10, 8, 12, 47, 0, TimeSpan.Zero));
		summary.LatestTime.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 47, 0, TimeSpan.Zero));
		summary.Duration.Should().Be(3600);
		summary.EventCount.Should().Be(5);
		summary.AdditionalProperties.Should().ContainKey("histogram");
		var linecount = summary.Fields["linecount"];
		linecount.Count.Should().Be(5);
		linecount.NumericCount.Should().Be(5);
		linecount.DistinctCount.Should().Be(1);
		linecount.IsExact.Should().BeTrue();
		linecount.Relevant.Should().BeFalse();
		linecount.Min.Should().Be("1");
		linecount.Max.Should().Be("1");
		linecount.Mean.Should().Be(1);
		linecount.StandardDeviation.Should().Be(0);
		linecount.AdditionalProperties.Should().BeEmpty();
		var mode = linecount.Modes.Should().ContainSingle().Subject;
		mode.Value.Should().Be("1");
		mode.Count.Should().Be(5);
		mode.IsExact.Should().BeTrue();
		summary.Fields["host"].Mean.Should().BeNull();
		summary.Fields["host"].Relevant.Should().BeTrue();
	}

	[Fact]
	public async Task GetSummaryAsync_NoContent_ReturnsNull()
	{
		using var client = TestClient.Create(SearchRequestAssert.NoContentStub());

		var summary = await client.SearchJobs.GetSummaryAsync("my_sid", null, Ct);

		summary.Should().BeNull();
	}

	[Fact]
	public async Task GetTimelineAsync_SendsGetWithTimeFormats()
	{
		var stub = TestClient.Stub(SearchJson.Timeline);
		using var client = TestClient.Create(stub);

		await client.SearchJobs.GetTimelineAsync("my_sid", new SearchJobTimelineOptions { OutputTimeFormat = "%s", TimeFormat = "%FT%T" }, Ct);

		stub.ShouldHaveSent(HttpMethod.Get, "/services/search/jobs/my_sid/timeline", "?output_time_format=%25s&time_format=%25FT%25T&output_mode=json");
	}

	[Fact]
	public async Task GetTimelineAsync_MapsTheBuckets()
	{
		using var client = TestClient.Create(TestClient.Stub(SearchJson.Timeline));

		var timeline = (await client.SearchJobs.GetTimelineAsync("my_sid", null, Ct))!;

		timeline.CursorTime.Should().BeNull();
		timeline.IsTimeCursored.Should().BeTrue();
		timeline.EventCount.Should().Be(5);
		timeline.Buckets.Should().HaveCount(2);
		var bucket = timeline.Buckets[1];
		bucket.TotalCount.Should().Be(5);
		bucket.AvailableCount.Should().Be(5);
		bucket.IsFinalized.Should().BeFalse();
		bucket.Duration.Should().Be(60);
		bucket.EarliestStrftime.Should().Be("2026-10-08T13:47:00.000+00:00");
		bucket.EarliestTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791467220));
		bucket.EarliestTimeOffset.Should().Be(3600);
		bucket.LatestTimeOffset.Should().Be(3600);
	}

	[Fact]
	public async Task GetTimelineAsync_NoContent_ReturnsNull()
	{
		using var client = TestClient.Create(SearchRequestAssert.NoContentStub());

		var timeline = await client.SearchJobs.GetTimelineAsync("my_sid", null, Ct);

		timeline.Should().BeNull();
	}
}
