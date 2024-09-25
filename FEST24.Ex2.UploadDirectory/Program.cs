using FEST24.Base;
using Relativity.Transfer.SDK;
using Relativity.Transfer.SDK.Interfaces.Paths;

namespace FEST24.Workshop.Exercise2;

internal static class Program
{
    private static readonly CancellationTokenProvider CancellationTokenProvider = new();
    
    private static async Task Main(string[] args)
    {
        try
        {
            ConsoleLogger.PrintInformation("Upload directory");
            
            await UploadDirectory();

            ConsoleLogger.PrintInformation("Upload directory completed. Press any key to exit.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            ConsoleLogger.PrintError(ex,"Upload directory failed");
        }
    }

    private static async Task UploadDirectory()
    {
        var jobId = Guid.NewGuid();

        var destination = new DirectoryPath(ExerciseConfiguration.DestinationRelativityFolderPath);
        var source = new DirectoryPath(ExerciseConfiguration.DirectoryToSend);
        var authenticationProvider = new ClientSecretAuthenticationProvider(
            new Uri(ExerciseConfiguration.RelativityAddress), ExerciseConfiguration.Credentials);

        var token = CancellationTokenProvider.GetCancellationToken();
        var progressHandler = TransferProgressHandlerProvider.Get();

        throw new NotImplementedException("Please provide way to upload directory using Transfer SDK"); 
    }
}