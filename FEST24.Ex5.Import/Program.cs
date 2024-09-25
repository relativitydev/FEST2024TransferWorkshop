using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FEST24.Base;
using FEST24.Ex5.Import.Helpers;
using Relativity.Import.V1;
using Relativity.Import.V1.Builders.DataSource;
using Relativity.Import.V1.Builders.Documents;
using Relativity.Import.V1.Models;
using Relativity.Import.V1.Models.Settings;
using Relativity.Import.V1.Models.Sources;

namespace FEST24.Ex5.Import;

internal static class Program
{
	private static readonly CancellationTokenProvider CancellationTokenProvider = new();

	private static async Task Main(string[] args)
	{
		try
		{
			ConsoleLogger.PrintInformation("Starting Exercise 5 - Import");
			ConsoleLogger.PrintColoredInformation("Before running this exercise, ensure that you have completed Exercise 4: 'Upload Items' and press any key to continue.", ConsoleColor.DarkRed);
			Console.Read();
			
			ConsoleLogger.PrintInformation("Import ");
			await ImportDocuments();

			ConsoleLogger.PrintInformation("\nImport completed - verify results via RelativityOne. Press any key to exit.");
			Console.ReadKey();
		}
		catch (Exception ex)
		{
			ConsoleLogger.PrintError(ex, "Exercise 5 failed. Error message:");
		}
	}

	private static async Task ImportDocuments()
	{
		var importId = Guid.NewGuid();
		var sourceId = Guid.NewGuid();

		var workspaceId = ExerciseConfiguration.WorkspaceId;

		var httpClient = await HttpClientHelper.CreateHttpClient();

		// Create Import Job
		await CreateImportJob(httpClient, workspaceId, importId);

		// Configure Document Settings
		// Other available options: image, rdo, production
		await AddImportDocumentSettings(httpClient, workspaceId, importId);
		await AddDataSourceSettings(httpClient, workspaceId, importId, sourceId);

		// Start Import
		await StartImportJob(httpClient, workspaceId, importId);

		// Monitor Progress
		await WaitForImportDataSourceToBeCompleted(httpClient, workspaceId, importId, sourceId);

		// Print Import Summary
		await GetImportDataSourceProgress(httpClient, workspaceId, importId, sourceId);

		// End Import
		await EndImportJob(httpClient, workspaceId, importId);
	}

	private static async Task CreateImportJob(HttpClient httpClient, int workspaceId, Guid importId)
	{
		var createJobPayload = new { applicationName = "Fest2024-Workshop-Ex5", correlationID = "Ex-5", };

		var createImportJobUri = RelativityImportEndpoints.GetImportJobCreateUri(workspaceId, importId);
		var response = await httpClient.PostAsJsonAsync(createImportJobUri, createJobPayload);
		await ImportJobHelper.EnsureSuccessResponse(response);

		ConsoleLogger.PrintInformation("Import job created");
	}

	private static async Task AddImportDocumentSettings(HttpClient httpClient, int workspaceId, Guid importId)
	{
		// set of columns indexes in load file used in import settings.
		const int controlNumberColumnIndex = 0;
		const int fileNameColumnIndex = 13;
		const int filePathColumnIndex = 22;

		const string overlayKeyField = "Control Number";

		ImportDocumentSettings importSettings = ImportDocumentSettingsBuilder.Create()
			.WithAppendOverlayMode(x => x
				.WithKeyField(overlayKeyField)
				.WithMultiFieldOverlayBehaviour(MultiFieldOverlayBehaviour.MergeAll))
			.WithNatives(x => x.WithFilePathDefinedInColumn(filePathColumnIndex).WithFileNameDefinedInColumn(fileNameColumnIndex))
			.WithoutImages()
			.WithFieldsMapped(x => x.WithField(controlNumberColumnIndex, "Control Number"))
			.WithoutFolders();

		var importSettingPayload = new { importSettings };

		var documentConfigurationUri = RelativityImportEndpoints.GetDocumentConfigurationUri(workspaceId, importId);
		var response = await httpClient.PostAsJsonAsync(documentConfigurationUri, importSettingPayload);
		await ImportJobHelper.EnsureSuccessResponse(response);
		ConsoleLogger.PrintInformation("Configured import job");
	}

	private static async Task AddDataSourceSettings(HttpClient httpClient, int workspaceId, Guid importId,
		Guid sourceId)
	{
		DataSourceSettings dataSourceSettings = DataSourceSettingsBuilder.Create().ForLoadFile(ExerciseConfiguration.LoadFilePath)
			.WithDelimiters(d => d
				.WithColumnDelimiters('|')
				.WithQuoteDelimiter('^')
				.WithNewLineDelimiter('#')
				.WithNestedValueDelimiter('&')
				.WithMultiValueDelimiter('$'))
			.WithFirstLineContainingHeaders()
			.WithEndOfLineForWindows()
			.WithStartFromBeginning()
			.WithDefaultEncoding()
			.WithDefaultCultureInfo();

		var dataSourceSettingsPayload = new { dataSourceSettings };

		var importSourcesUri = RelativityImportEndpoints.GetImportSourceUri(workspaceId, importId, sourceId);
		var response = await httpClient.PostAsJsonAsync(importSourcesUri, dataSourceSettingsPayload);
		await ImportJobHelper.EnsureSuccessResponse(response);
		ConsoleLogger.PrintInformation("Configured data source settings");
	}

	private static async Task StartImportJob(HttpClient httpClient, int workspaceId, Guid importId)
	{
		var beginImportJobUri = RelativityImportEndpoints.GetImportJobBeginUri(workspaceId, importId);
		var response = await httpClient.PostAsync(beginImportJobUri, null);
		await ImportJobHelper.EnsureSuccessResponse(response);
		ConsoleLogger.PrintInformation("Import job started");
	}

	private static async Task WaitForImportDataSourceToBeCompleted(HttpClient httpClient, int workspaceId, Guid importId, Guid sourceId)
	{
		var importSourceDetailsUri =
			RelativityImportEndpoints.GetImportSourceDetailsUri(workspaceId, importId, sourceId);

		JsonSerializerOptions options = new() { Converters = { new JsonStringEnumConverter() } };

		await ImportJobHelper.WaitImportDataSourceToBeCompleted(funcAsync: () => 
			httpClient.GetFromJsonAsync<ValueResponse<DataSourceDetails>>(importSourceDetailsUri, options), timeout: 1200000);
	}

	private static async Task GetImportDataSourceProgress(HttpClient httpClient, int workspaceId, Guid importId, Guid sourceId)
	{
		var importSourceProgressUri = RelativityImportEndpoints.GetImportSourceProgressUri(workspaceId, importId, sourceId);
		var valueResponse = await httpClient.GetFromJsonAsync<ValueResponse<ImportProgress>>(importSourceProgressUri);

		if (valueResponse?.IsSuccess ?? false)
		{
			ConsoleLogger.PrintInformation($"Import data source progress: Total records: {valueResponse.Value.TotalRecords}," +
			                  $" Imported records: {valueResponse.Value.ImportedRecords}, Records with errors: {valueResponse.Value.ErroredRecords}");
		}
	}

	private static async Task EndImportJob(HttpClient httpClient, int workspaceId, Guid importId)
	{
		var endImportJobUri = RelativityImportEndpoints.GetImportJobEndUri(workspaceId, importId);
		var response = await httpClient.PostAsync(endImportJobUri, null);
		await ImportJobHelper.EnsureSuccessResponse(response);
		ConsoleLogger.PrintInformation("Import job ended");
	}
}