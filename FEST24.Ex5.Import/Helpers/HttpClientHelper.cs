using System.Net.Http.Json;
using System.Text.Json;
using FEST24.Base;
using System.Net.Http.Headers;

namespace FEST24.Ex5.Import.Helpers;

internal static class HttpClientHelper
{
	internal static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response) where T : class
	{
		if (response.IsSuccessStatusCode)
		{
			try
			{
				return await response.Content.ReadFromJsonAsync<T>();
			}
			catch (JsonException jsonException)
			{
				ConsoleLogger.PrintInformation($"JsonExceptionJson occurred during response deserialization: {jsonException}");
			}
			catch (Exception ex)
			{
				ConsoleLogger.PrintError(ex, "Exception occurred during response deserialization:");
			}

			return null;
		}

		return null;
	}

	internal static async Task<HttpClient> CreateHttpClient()
	{
		var baseAddress = ExerciseConfiguration.RelativityAddress + "/Relativity.REST/";
		var httpClient = new HttpClient
		{
			BaseAddress = new Uri(baseAddress),
		};
		var bearerTokenProvider = new BearerTokenProvider();

		var bearerToken = await bearerTokenProvider.GetBearerTokenAsync(new Uri(ExerciseConfiguration.RelativityAddress), ExerciseConfiguration.Credentials).ConfigureAwait(false);
		httpClient.DefaultRequestHeaders.Add("X-CSRF-Header", "."); 
		httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

		return httpClient;
	}
}