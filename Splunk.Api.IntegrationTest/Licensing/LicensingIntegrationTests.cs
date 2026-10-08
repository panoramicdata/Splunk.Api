using System.Net;

namespace Splunk.Api.IntegrationTest.Licensing;

/// <summary>Licensing is read-only here: the shared instance's licenses, groups and pools are never changed.</summary>
[Collection(SplunkTestGroup.Name)]
public class LicensingIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task Groups_ExactlyOneIsActive_AndCanBeRead()
	{
		var ct = TestContext.Current.CancellationToken;

		var groups = await fixture.Client.LicenseGroups.ListAsync(null, ct);

		var active = groups.Entries.Should().ContainSingle(e => e.Content!.IsActive).Subject;
		active.Content!.StackIds.Should().NotBeEmpty();
		var one = await fixture.Client.LicenseGroups.GetAsync(active.Name, ct);
		one.Entries.Should().ContainSingle().Which.Content!.IsActive.Should().BeTrue();
	}

	[Fact]
	public async Task Licenses_ListAndGet()
	{
		var ct = TestContext.Current.CancellationToken;

		var licenses = await fixture.Client.Licenses.ListAsync(null, ct);

		var first = licenses.Entries.Should().NotBeEmpty().And.Subject.First();
		first.Content!.LicenseHash.Should().Be(first.Name);
		first.Content.Quota.Should().BePositive();
		first.Content.Status.Should().BeOneOf("VALID", "EXPIRED");
		var one = await fixture.Client.Licenses.GetAsync(first.Name, ct);
		one.Entries.Should().ContainSingle().Which.Content!.StackId.Should().Be(first.Content.StackId);
	}

	[Fact]
	public async Task StacksPoolsAndPeers_ListAndGet()
	{
		var ct = TestContext.Current.CancellationToken;

		var stacks = await fixture.Client.LicenseStacks.ListAsync(null, ct);
		var pools = await fixture.Client.LicensePools.ListAsync(null, ct);
		var peers = await fixture.Client.LicensePeers.ListAsync(null, ct);

		var stack = stacks.Entries.Should().NotBeEmpty().And.Subject.First();
		var pool = pools.Entries.Should().NotBeEmpty().And.Subject.First();
		var peer = peers.Entries.Should().NotBeEmpty().And.Subject.First();
		(await fixture.Client.LicenseStacks.GetAsync(stack.Name, ct)).Entries.Should().ContainSingle().Which.Content!.Quota.Should().BePositive();
		(await fixture.Client.LicensePools.GetAsync(pool.Name, ct)).Entries.Should().ContainSingle().Which.Content!.StackId.Should().NotBeNullOrWhiteSpace();
		(await fixture.Client.LicensePeers.GetAsync(peer.Name, ct)).Entries.Should().ContainSingle().Which.Content!.PoolIds.Should().NotBeEmpty();
	}

	[Fact]
	public async Task LocalPeerAndUsage_AreReported()
	{
		var ct = TestContext.Current.CancellationToken;

		var local = await fixture.Client.LicenseLocalPeer.GetAsync(ct);
		var usage = await fixture.Client.LicenseUsage.GetAsync(null, ct);

		var peer = local.Entries.Should().ContainSingle().Subject.Content!;
		peer.PeerId.Should().NotBeNullOrWhiteSpace();
		peer.Features.Should().NotBeEmpty();
		peer.LicenseKeys.Should().NotBeEmpty();
		usage.Entries.Should().ContainSingle().Which.Content!.Quota.Should().BePositive();
	}

	[Fact]
	public async Task Messages_List_AndAnUnknownIdIsNotFound()
	{
		var ct = TestContext.Current.CancellationToken;

		var messages = await fixture.Client.LicenseMessages.ListAsync(null, ct);

		messages.Entries.Should().OnlyContain(e => e.Content!.Severity != null);
		var act = () => fixture.Client.LicenseMessages.GetAsync("splunk_api_it_missing", ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
