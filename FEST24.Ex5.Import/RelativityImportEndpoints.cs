namespace FEST24.Ex5.Import;

internal static class RelativityImportEndpoints
{
	public static string GetImportJobCreateUri(int workspaceId, Guid importId) => $"api/import-service/v1/workspaces/{workspaceId}/import-jobs/{importId}";
	public static string GetDocumentConfigurationUri(int workspaceId, Guid importId) => $"api/import-service/v1/workspaces/{workspaceId}/import-jobs/{importId}/documents-configurations";
	public static string GetImportSourceUri(int workspaceId, Guid importId, Guid sourceId) => $"api/import-service/v1/workspaces/{workspaceId}/import-jobs/{importId}/sources/{sourceId}";
	public static string GetImportJobBeginUri(int workspaceId, Guid importId) => $"api/import-service/v1/workspaces/{workspaceId}/import-jobs/{importId}/begin";
	public static string GetImportSourceProgressUri(int workspaceId, Guid importId, Guid sourceId) => $"api/import-service/v1/workspaces/{workspaceId}/import-jobs/{importId}/sources/{sourceId}/progress";
	public static string GetImportJobEndUri(int workspaceId, Guid importId) => $"api/import-service/v1/workspaces/{workspaceId}/import-jobs/{importId}/end";
	public static string GetImportSourceDetailsUri(int workspaceId, Guid importId, Guid sourceId) => $"api/import-service/v1/workspaces/{workspaceId}/import-jobs/{importId}/sources/{sourceId}/details";
}