using FEST24.Base;
using Relativity.Transfer.SDK;
using Relativity.Transfer.SDK.Interfaces.Paths;

namespace FEST24.Ex4.UploadItems;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            ConsoleLogger.PrintInformation("Upload items");

            await UploadItems();

            ConsoleLogger.PrintInformation("Upload items completed. Press any key to exit.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            ConsoleLogger.PrintError(ex, "Upload items failed");
        }
    }

    private static async Task UploadItems()
    {
        var jobId = Guid.NewGuid();
        
        var clientName = ExerciseConfiguration.ClientName;
        var source = new DirectoryPath(ExerciseConfiguration.UploadItemsFilePath);
        var destination = new DirectoryPath(ExerciseConfiguration.DestinationRelativityFolderPath);
        var authenticationProvider = new ClientSecretAuthenticationProvider(new Uri(ExerciseConfiguration.RelativityAddress), ExerciseConfiguration.Credentials);
        var progressHandler = TransferProgressHandlerProvider.Get();
        
        throw new NotImplementedException("Please provide way to upload items using Transfer SDK");
    }

    private static async IAsyncEnumerable<TransferEntity> GetTransferredEntities(string filePath)
    {
        var lines = await File.ReadAllLinesAsync(filePath);

        foreach (var (line, index) in lines.Select((x, y) => (x, y)))
        {
            var paths = line.Split(';');
            if (paths.Length != 2)
            {
                ConsoleLogger.PrintInformation($"Invalid parameters in {index} line");
                continue;
            }

            var transferEntity = new TransferEntity(new FilePath(paths[0]), new FilePath(paths[1]));

            yield return transferEntity;
        }
    }
}