using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Spl2;

namespace Splunk.Api.Interfaces;

/// <summary>SPL2 modules, their permissions and dispatch (<c>orchestrator/v1/spl2/modules</c>).</summary>
/// <remarks>
/// <para>
/// The module and permission endpoints do not exist under <c>servicesNS</c> (Splunk answers 404 <c>route not found.</c>):
/// call them from a client without a namespace, and name the namespace in the request instead.
/// </para>
/// <para>
/// Creating a module and dispatching run in Splunk's SPL2 language server. Where that service is not running (as in the
/// <c>splunk/splunk</c> 10.6 Docker image), Splunk answers 500 with <c>Failed to open resource handle: uds:///.../lsp-N.sock</c>.
/// </para>
/// </remarks>
public interface ISpl2Modules
{
	/// <summary>Lists the modules the caller can read (<c>GET orchestrator/v1/spl2/modules</c>).</summary>
	/// <param name="options">Namespace, filters and paging; <see langword="null"/> for every module.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The modules.</returns>
	[Get("services/orchestrator/v1/spl2/modules")]
	Task<Spl2ModuleList> ListAsync([Query] Spl2ModuleListOptions? options, CancellationToken cancellationToken);

	/// <summary>Gets a module (<c>GET orchestrator/v1/spl2/modules/{resourceName}</c>).</summary>
	/// <param name="resourceName">The module's fully qualified name, for example <c>apps.search.my_module</c>.</param>
	/// <param name="includeAnnotations">Whether to return annotations (<c>include_annotations</c>).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The module.</returns>
	[Get("services/orchestrator/v1/spl2/modules/{resourceName}")]
	Task<Spl2Module> GetAsync(string resourceName, [AliasAs("include_annotations")] bool? includeAnnotations, CancellationToken cancellationToken);

	/// <summary>Creates or replaces a module (<c>PUT orchestrator/v1/spl2/modules/{resourceName}</c>).</summary>
	/// <param name="resourceName">The module's fully qualified name, for example <c>apps.search.my_module</c>.</param>
	/// <param name="isUpdate">Whether to replace an existing module (<c>isUpdate</c>); without it an existing module raises 409.</param>
	/// <param name="includeAnnotations">Whether to return annotations (<c>include_annotations</c>).</param>
	/// <param name="request">The module.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>The module as stored.</returns>
	[Put("services/orchestrator/v1/spl2/modules/{resourceName}")]
	Task<Spl2Module> PutAsync(
		string resourceName,
		[AliasAs("isUpdate")] bool? isUpdate,
		[AliasAs("include_annotations")] bool? includeAnnotations,
		[Body] JsonBody<Spl2ModuleRequest> request,
		CancellationToken cancellationToken);

	/// <summary>Deletes a module (<c>DELETE orchestrator/v1/spl2/modules/{resourceName}</c>).</summary>
	/// <param name="resourceName">The module's fully qualified name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the module is deleted (204).</returns>
	[Delete("services/orchestrator/v1/spl2/modules/{resourceName}")]
	Task DeleteAsync(string resourceName, CancellationToken cancellationToken);

	/// <summary>Starts a module's named search statements (<c>POST orchestrator/v1/spl2/modules/dispatch</c>).</summary>
	/// <param name="request">The module source and the statements to run.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>Each statement's search job.</returns>
	[Post("services/orchestrator/v1/spl2/modules/dispatch")]
	Task<Spl2DispatchResult> DispatchAsync([Body] JsonBody<Spl2DispatchRequest> request, CancellationToken cancellationToken);

	/// <summary>Gets a module's permissions (<c>GET orchestrator/v1/spl2/modules/permissions</c>).</summary>
	/// <param name="resourceName">The module's fully qualified name (<c>resourceName</c>).</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>One entry per role.</returns>
	[Get("services/orchestrator/v1/spl2/modules/permissions")]
	Task<IReadOnlyList<Spl2ModuleRoleAccess>> GetPermissionsAsync([AliasAs("resourceName")] string resourceName, CancellationToken cancellationToken);

	/// <summary>Replaces a module's permissions (<c>PUT orchestrator/v1/spl2/modules/permissions</c>).</summary>
	/// <param name="resourceName">The module's fully qualified name (<c>resourceName</c>).</param>
	/// <param name="request">The roles granted each operation.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns><c>{"code":201}</c> on success.</returns>
	/// <remarks>An unknown module raises 400 (<c>failed to find module ...</c>), not 404.</remarks>
	[Put("services/orchestrator/v1/spl2/modules/permissions")]
	Task<Spl2PermissionsUpdateResult> UpdatePermissionsAsync(
		[AliasAs("resourceName")] string resourceName,
		[Body] JsonBody<Spl2ModulePermissionsRequest> request,
		CancellationToken cancellationToken);
}
