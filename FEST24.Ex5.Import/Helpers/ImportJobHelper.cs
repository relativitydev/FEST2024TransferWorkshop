using FEST24.Base;
using Relativity.Import.V1;
using Relativity.Import.V1.Models.Sources;

namespace FEST24.Ex5.Import.Helpers;

internal static class ImportJobHelper
{
	internal static async Task EnsureSuccessResponse(HttpResponseMessage message)
	{
		ConsoleLogger.PrintColoredInformation($"{message.RequestMessage?.Method} {message.RequestMessage?.RequestUri?.PathAndQuery}", ConsoleColor.DarkGreen);

		message.EnsureSuccessStatusCode();
		var response = await HttpClientHelper.DeserializeResponse<Response>(message);

		if (response == null)
		{
			var errorInfo = "Deserialized response model is null";
			
			ConsoleLogger.PrintInformation(errorInfo);
			throw new Exception(errorInfo);
		}

		if (response.IsSuccess == false)
		{
			var errorInfo = $"ErrorMessage: {response.ErrorMessage} ErrorCode: {response.ErrorCode}";
			ConsoleLogger.PrintInformation($"Response.IsSuccess: {response.IsSuccess} {errorInfo}");

			throw new Exception($"Response failed: {errorInfo}");
		}

		ConsoleLogger.PrintInformation($"Response.IsSuccess: {response.IsSuccess}");
	}
	internal static async Task<DataSourceState?> WaitImportDataSourceToBeCompleted(Func<Task<ValueResponse<DataSourceDetails>?>> funcAsync, int? timeout = null)
	{
		DataSourceState[] completedStates = { DataSourceState.Completed, DataSourceState.CompletedWithItemErrors, DataSourceState.Failed };
		DataSourceState? state = null;
		var timeoutTask = Task.Delay(timeout ?? Timeout.Infinite);
		do
		{
			await Task.Delay(5000);
			var valueResponse = await funcAsync();
			if (valueResponse is { IsSuccess: true })
			{
				state = valueResponse.Value.State;
				ConsoleLogger.PrintInformation($"DataSource state: {state}");
			}
		}
		while (completedStates.All(x => x != state) && !timeoutTask.IsCompleted);

		return state;
	}
}