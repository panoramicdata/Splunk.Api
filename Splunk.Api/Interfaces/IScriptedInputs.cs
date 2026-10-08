using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.Interfaces;

/// <summary>Scripted inputs (<c>data/inputs/script</c>). The name of each is the script's path, with any arguments.</summary>
public interface IScriptedInputs
{
	/// <summary>Lists the scripted inputs (<c>GET data/inputs/script</c>).</summary>
	/// <param name="options">Paging and filtering, or <see langword="null"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per input.</returns>
	[Get("services/data/inputs/script")]
	Task<SplunkFeed<ScriptedInput>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a scripted input (<c>POST data/inputs/script</c>).</summary>
	/// <remarks>Splunk 10.6 answers with an empty feed; read the input back with <see cref="GetAsync"/>.</remarks>
	/// <param name="request">The input.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed, empty in Splunk 10.6.</returns>
	[Post("services/data/inputs/script")]
	Task<SplunkFeed<ScriptedInput>> CreateAsync([Body] ScriptedInputCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Restarts a scripted input (<c>POST data/inputs/script/restart</c>).</summary>
	/// <param name="request">The script to restart.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed, empty in Splunk 10.6.</returns>
	[Post("services/data/inputs/script/restart")]
	Task<SplunkFeed<ScriptedInput>> RestartAsync([Body] ScriptRestartRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a scripted input (<c>GET data/inputs/script/{name}</c>).</summary>
	/// <param name="name">The script's path, exactly as listed.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the input.</returns>
	[Get("services/data/inputs/script/{name}")]
	Task<SplunkFeed<ScriptedInput>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Changes a scripted input (<c>POST data/inputs/script/{name}</c>).</summary>
	/// <param name="name">The script's path.</param>
	/// <param name="request">The new settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the inputs.</returns>
	[Post("services/data/inputs/script/{name}")]
	Task<SplunkFeed<ScriptedInput>> UpdateAsync(string name, [Body] ScriptedInputUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a scripted input (<c>DELETE data/inputs/script/{name}</c>).</summary>
	/// <param name="name">The script's path.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the input is deleted.</returns>
	[Delete("services/data/inputs/script/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
