using Splunk.Api.Models.FederatedSearch;
using System.Net;

namespace Splunk.Api.IntegrationTest.FederatedSearch;

[Collection(SplunkTestGroup.Name)]
public class FederatedSearchIntegrationTests(SplunkFixture fixture)
{
	// A documentation address (RFC 5737): never routable, so the provider can never be searched or connect anywhere.
	private const string UnroutableHostPort = "192.0.2.1:8089";

	[Fact]
	public async Task SettingsGetAsync_ReturnsTheGeneralSettings()
	{
		var feed = await fixture.Client.FederatedSearchSettings.GetAsync(TestContext.Current.CancellationToken);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("general");
		entry.Content!.TransparentMode.Should().NotBeNull();
		entry.Content.ProxyBundlesTtl.Should().BePositive();
	}

	[Fact]
	public async Task ProvidersListAsync_Succeeds()
	{
		var feed = await fixture.Client.FederatedProviders.ListAsync(null, TestContext.Current.CancellationToken);

		feed.Entries.Should().OnlyContain(e => e.Content!.Type != FederatedProviderType.Unknown);
	}

	[Fact]
	public async Task IndexesListAsync_Succeeds()
	{
		var feed = await fixture.Client.FederatedIndexes.ListAsync(null, TestContext.Current.CancellationToken);

		feed.Paging!.Total.Should().Be(feed.Entries.Count);
	}

	[Fact]
	public async Task ProvidersDisableAllAsync_AmazonS3OnSplunkEnterprise_Raises400()
	{
		// Only the Amazon S3 variant is safe here: there can be no S3 providers on Splunk Enterprise, so nothing is turned off.
		var act = () => fixture.Client.FederatedProviders.DisableAllAsync(
			new FederatedProviderBatchDisableRequest { Type = FederatedProviderType.AwsS3 },
			TestContext.Current.CancellationToken);

		var error = (await act.Should().ThrowAsync<SplunkApiException>()).Which;
		error.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		error.Message.Should().StartWith("Federated Search for Amazon S3 is disabled.");
	}

	[Fact]
	public async Task ProviderAndIndex_RoundTrip()
	{
		var ct = TestContext.Current.CancellationToken;
		var providerName = SplunkFixture.UniqueName("provider");
		var indexName = "federated:" + SplunkFixture.UniqueName("index");
		var providers = fixture.Client.FederatedProviders;
		var indexes = fixture.Client.FederatedIndexes;
		try
		{
			var created = await providers.CreateAsync(
				new FederatedProviderCreateRequest
				{
					Name = providerName,
					Type = FederatedProviderType.Splunk,
					Mode = FederatedProviderMode.Standard,
					HostPort = UnroutableHostPort,
					ServiceAccount = "svc",
					Password = "not-a-real-password",
					AppContext = "search"
				},
				ct);
			var provider = created.Entries.Should().ContainSingle().Subject.Content!;
			provider.Type.Should().Be(FederatedProviderType.Splunk);
			provider.HostPort.Should().Be(UnroutableHostPort);

			(await providers.UpdateAsync(providerName, new FederatedProviderUpdateRequest { ServiceAccount = "svc2" }, ct))
				.Entries.Should().ContainSingle().Which.Content!.ServiceAccount.Should().Be("svc2");
			(await providers.DisableAsync(providerName, ct)).Entries.Should().ContainSingle().Which.Content!.Disabled.Should().BeTrue();
			(await providers.EnableAsync(providerName, ct)).Entries.Should().ContainSingle().Which.Content!.Disabled.Should().BeFalse();
			(await providers.GetAsync(providerName, ct)).Entries.Should().ContainSingle().Which.Content!.Mode.Should().Be(FederatedProviderMode.Standard);

			var index = (await indexes.CreateAsync(new FederatedIndexCreateRequest { Name = indexName, Provider = providerName, Dataset = "index:main" }, ct))
				.Entries.Should().ContainSingle().Subject;
			index.Name.Should().Be(indexName);
			index.Content!.Provider.Should().Be(providerName);

			(await indexes.UpdateAsync(indexName, new FederatedIndexUpdateRequest { Dataset = "index:_internal" }, ct))
				.Entries.Should().ContainSingle().Which.Content!.Dataset.Should().Be("index:_internal");
			(await indexes.DisableAsync(indexName, ct)).Entries.Should().ContainSingle().Which.Content!.Disabled.Should().BeTrue();
			(await indexes.EnableAsync(indexName, ct)).Entries.Should().ContainSingle().Which.Content!.Disabled.Should().BeFalse();
			(await indexes.GetAsync(indexName, ct)).Entries.Should().ContainSingle().Which.Content!.Dataset.Should().Be("index:_internal");
		}
		finally
		{
			await DeleteQuietlyAsync(() => indexes.DeleteAsync(indexName, CancellationToken.None));
			await DeleteQuietlyAsync(() => providers.DeleteAsync(providerName, CancellationToken.None));
		}

		var gone = () => providers.GetAsync(providerName, ct);
		(await gone.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Be($"Could not find object id=provider://{providerName}");
	}

	private static async Task DeleteQuietlyAsync(Func<Task> delete)
	{
		try
		{
			await delete();
		}
		catch (SplunkApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
		{
			// Already gone (the create failed): nothing to clean up.
		}
	}
}
