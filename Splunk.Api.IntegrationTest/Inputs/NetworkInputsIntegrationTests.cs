using Splunk.Api.Models;
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
		await PortInputRoundTripAsync(
			port,
			async () =>
			{
				var created = await inputs.CreateAsync(new RawTcpInputCreateRequest { Name = port, Sourcetype = sourcetype, ConnectionHost = ConnectionHost.Ip, Disabled = true }, Ct);
				created.Entries.Should().ContainSingle().Which.Content!.ConnectionHost.Should().Be(ConnectionHost.Ip);

				var updated = await inputs.UpdateAsync(port, new RawTcpInputUpdateRequest { ConnectionHost = ConnectionHost.None, RawTcpDoneTimeout = 7 }, Ct);
				updated.Entries.Single().Content!.RawTcpDoneTimeout.Should().Be(7);

				var read = (await inputs.GetAsync(port, Ct)).Entries.Single().Content!;
				read.ConnectionHost.Should().Be(ConnectionHost.None);
				read.Sourcetype.Should().Be(sourcetype);
			},
			inputs.ListConnectionsAsync,
			inputs.DeleteAsync,
			inputs.GetAsync);
	}

	[Fact]
	public async Task CookedTcpInput_RoundTrip()
	{
		var port = RandomPort();
		var inputs = fixture.Client.CookedTcpInputs;
		await PortInputRoundTripAsync(
			port,
			async () =>
			{
				var created = await inputs.CreateAsync(new CookedTcpInputCreateRequest { Name = port, ConnectionHost = ConnectionHost.Dns, Disabled = true }, Ct);
				created.Entries.Should().ContainSingle().Which.Name.Should().Be(port);

				await inputs.UpdateAsync(port, new CookedTcpInputUpdateRequest { ConnectionHost = ConnectionHost.Ip, Disabled = true }, Ct);

				(await inputs.GetAsync(port, Ct)).Entries.Single().Content!.ConnectionHost.Should().Be(ConnectionHost.Ip);
			},
			inputs.ListConnectionsAsync,
			inputs.DeleteAsync,
			inputs.GetAsync);
	}

	[Fact]
	public async Task UdpInput_RoundTrip()
	{
		var port = RandomPort();
		var sourcetype = SplunkFixture.UniqueName("udp");
		var inputs = fixture.Client.UdpInputs;
		await PortInputRoundTripAsync(
			port,
			async () =>
			{
				var created = await inputs.CreateAsync(new UdpInputCreateRequest { Name = port, Sourcetype = sourcetype, NoAppendingTimestamp = true, Disabled = true }, Ct);
				created.Entries.Should().ContainSingle().Which.Content!.NoAppendingTimestamp.Should().BeTrue();

				await inputs.UpdateAsync(port, new UdpInputUpdateRequest { NoPriorityStripping = true }, Ct);

				var read = (await inputs.GetAsync(port, Ct)).Entries.Single().Content!;
				read.NoPriorityStripping.Should().BeTrue();
				read.Sourcetype.Should().Be(sourcetype);
			},
			inputs.ListConnectionsAsync,
			inputs.DeleteAsync,
			inputs.GetAsync);
	}

	[Fact]
	public async Task SplunkTcpToken_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("tcptoken");
		var value = Guid.NewGuid().ToString();
		var tokens = fixture.Client.SplunkTcpTokens;
		await RoundTripAsync(
			async () =>
			{
				var created = await tokens.CreateAsync(new SplunkTcpTokenCreateRequest { Name = name, Token = value }, Ct);
				created.Entries.Should().ContainSingle().Which.Name.Should().Be("splunktcptoken://" + name);

				var replacement = Guid.NewGuid().ToString();
				await tokens.UpdateAsync(name, new SplunkTcpTokenUpdateRequest { Token = replacement }, Ct);

				(await tokens.GetAsync(name, Ct)).Entries.Single().Content!.Token.Should().Be(replacement);
			},
			() => tokens.DeleteAsync(name, Ct),
			() => tokens.GetAsync(name, Ct));
	}

	/// <summary>Round trip of an input on <paramref name="port"/>, which nothing sends to, so it has no connections.</summary>
	private static Task PortInputRoundTripAsync(
		string port,
		Func<Task> exercise,
		Func<string, CancellationToken, Task<SplunkFeed<InputConnection>>> listConnections,
		Func<string, CancellationToken, Task> delete,
		Func<string, CancellationToken, Task> read)
		=> RoundTripAsync(
			async () =>
			{
				await exercise();
				(await listConnections(port, Ct)).Entries.Should().BeEmpty();
			},
			() => delete(port, Ct),
			() => read(port, Ct));

	/// <summary>Runs <paramref name="exercise"/>, always deletes the object, then checks that reading it answers 404.</summary>
	private static async Task RoundTripAsync(Func<Task> exercise, Func<Task> delete, Func<Task> read)
	{
		try
		{
			await exercise();
		}
		finally
		{
			await delete();
		}

		(await read.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
