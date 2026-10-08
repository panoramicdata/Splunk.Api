using Splunk.Api.Models;
using Splunk.Api.Models.Access;
using System.Net;

namespace Splunk.Api.IntegrationTest.Access;

[Collection(SplunkTestGroup.Name)]
public class StoragePasswordsIntegrationTests(SplunkFixture fixture)
{
	[Fact]
	public async Task CreateGetUpdateDelete_RoundTrips()
	{
		var ct = TestContext.Current.CancellationToken;
		var passwords = fixture.Client.InNamespace("nobody", "search").StoragePasswords;
		var realm = SplunkFixture.UniqueName("realm");
		var user = "splunk_api_it_user";
		var name = $"{realm}:{user}:";
		var secret = "It-" + Guid.NewGuid().ToString("N");
		try
		{
			var created = await passwords.CreateAsync(new StoredPasswordCreateRequest { Name = user, Password = secret, Realm = realm }, ct);
			var entry = created.Entries.Should().ContainSingle().Subject;
			entry.Name.Should().Be(name);
			entry.Content!.ClearPassword.Should().BeNull("Splunk does not echo the clear-text password on create");
			entry.Content.Password.Should().Be("********");
			entry.Content.EncryptedPassword.Should().NotBeNullOrWhiteSpace();

			var read = (await passwords.GetAsync(name, ct)).Entries.Should().ContainSingle().Subject.Content!;
			read.ClearPassword.Should().Be(secret);
			read.Realm.Should().Be(realm);
			read.Username.Should().Be(user);

			var newSecret = "It-" + Guid.NewGuid().ToString("N");
			await passwords.UpdateAsync(name, new StoredPasswordUpdateRequest { Password = newSecret }, ct);
			(await passwords.GetAsync(name, ct)).Entries[0].Content!.ClearPassword.Should().Be(newSecret);

			var listed = await passwords.ListAsync(new ListOptions { Search = realm, Count = 0 }, ct);
			listed.Entries.Should().ContainSingle().Which.Name.Should().Be(name);
		}
		finally
		{
			await passwords.DeleteAsync(name, ct);
		}

		var act = () => passwords.GetAsync(name, ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
