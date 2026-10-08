using Splunk.Api.Models.Inputs;
using System.Net;

namespace Splunk.Api.IntegrationTest.Inputs;

/// <summary>
/// Round trips for TCP and UDP inputs on random high ports (the shared instance does not publish them), and for a
/// receiver token. Ports cannot carry the test prefix, so the inputs' sourcetype does.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class NetworkInputsIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private static string RandomPort() => Random.Shared.Next(40000, 49000).ToString(System.Globalization.CultureInfo.InvariantCulture);

	[Fact]
	public async Task RawTcpInput_RoundTrip()
	{
		var port = RandomPort();
		var sourcetype = SplunkFixture.UniqueName("tcp");
		var inputs = fixture.Client.RawTcpInputs;
		try
		{
			var created = await inputs.CreateAsync(new RawTcpInputCreateRequest { Name = port, Sourcetype = sourcetype, ConnectionHost = ConnectionHost.Ip, Disabled = true }, Ct);
			created.Entries.Should().ContainSingle().Which.Content!.ConnectionHost.Should().Be(ConnectionHost.Ip);

			var updated = await inputs.UpdateAsync(port, new RawTcpInputUpdateRequest { ConnectionHost = ConnectionHost.None, RawTcpDoneTimeout = 7 }, Ct);
			updated.Entries.Single().Content!.RawTcpDoneTimeout.Should().Be(7);

			var read = (await inputs.GetAsync(port, Ct)).Entries.Single().Content!;
			read.ConnectionHost.Should().Be(ConnectionHost.None);
			read.Sourcetype.Should().Be(sourcetype);
			(await inputs.ListConnectionsAsync(port, Ct)).Entries.Should().BeEmpty();
		}
		finally
		{
			await inputs.DeleteAsync(port, Ct);
		}

		await AssertGoneAsync(() => inputs.GetAsync(port, Ct));
	}

	[Fact]
	public async Task CookedTcpInput_RoundTrip()
	{
		var port = RandomPort();
		var inputs = fixture.Client.CookedTcpInputs;
		try
		{
			var created = await inputs.CreateAsync(new CookedTcpInputCreateRequest { Name = port, ConnectionHost = ConnectionHost.Dns, Disabled = true }, Ct);
			created.Entries.Should().ContainSingle().Which.Name.Should().Be(port);

			await inputs.UpdateAsync(port, new CookedTcpInputUpdateRequest { ConnectionHost = ConnectionHost.Ip, Disabled = true }, Ct);

			(await inputs.GetAsync(port, Ct)).Entries.Single().Content!.ConnectionHost.Should().Be(ConnectionHost.Ip);
			(await inputs.ListConnectionsAsync(port, Ct)).Entries.Should().BeEmpty();
		}
		finally
		{
			await inputs.DeleteAsync(port, Ct);
		}

		await AssertGoneAsync(() => inputs.GetAsync(port, Ct));
	}

	[Fact]
	public async Task UdpInput_RoundTrip()
	{
		var port = RandomPort();
		var sourcetype = SplunkFixture.UniqueName("udp");
		var inputs = fixture.Client.UdpInputs;
		try
		{
			var created = await inputs.CreateAsync(new UdpInputCreateRequest { Name = port, Sourcetype = sourcetype, NoAppendingTimestamp = true, Disabled = true }, Ct);
			created.Entries.Should().ContainSingle().Which.Content!.NoAppendingTimestamp.Should().BeTrue();

			await inputs.UpdateAsync(port, new UdpInputUpdateRequest { NoPriorityStripping = true }, Ct);

			var read = (await inputs.GetAsync(port, Ct)).Entries.Single().Content!;
			read.NoPriorityStripping.Should().BeTrue();
			read.Sourcetype.Should().Be(sourcetype);
			(await inputs.ListConnectionsAsync(port, Ct)).Entries.Should().BeEmpty();
		}
		finally
		{
			await inputs.DeleteAsync(port, Ct);
		}

		await AssertGoneAsync(() => inputs.GetAsync(port, Ct));
	}

	[Fact]
	public async Task SplunkTcpToken_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("tcptoken");
		var value = Guid.NewGuid().ToString();
		var tokens = fixture.Client.SplunkTcpTokens;
		try
		{
			var created = await tokens.CreateAsync(new SplunkTcpTokenCreateRequest { Name = name, Token = value }, Ct);
			created.Entries.Should().ContainSingle().Which.Name.Should().Be("splunktcptoken://" + name);

			var replacement = Guid.NewGuid().ToString();
			await tokens.UpdateAsync(name, new SplunkTcpTokenUpdateRequest { Token = replacement }, Ct);

			(await tokens.GetAsync(name, Ct)).Entries.Single().Content!.Token.Should().Be(replacement);
		}
		finally
		{
			await tokens.DeleteAsync(name, Ct);
		}

		await AssertGoneAsync(() => tokens.GetAsync(name, Ct));
	}

	private static async Task AssertGoneAsync(Func<Task> read)
		=> (await read.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
}
