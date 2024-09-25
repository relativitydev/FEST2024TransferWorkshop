using FEST24.Base;
using Relativity.Transfer.SDK;
using Relativity.Transfer.SDK.Interfaces.Options;
using Relativity.Transfer.SDK.Interfaces.Options.Policies;
using Relativity.Transfer.SDK.Interfaces.Paths;

namespace FEST24.Workshop.Exercise3;

internal static class Program
{
    private static readonly CancellationTokenProvider CancellationTokenProvider = new();

    private static async Task Main(string[] args)
    {
        try
        {
            ConsoleLogger.PrintInformation("Download directory");

            await DownloadDirectory();

            ConsoleLogger.PrintInformation("Download directory completed. Press any key to exit.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            ConsoleLogger.PrintError(ex, "Download directory failed");
        }
    }

    private static async Task DownloadDirectory()
    {
        var jobId = Guid.NewGuid();
        
        var source = new DirectoryPath(ExerciseConfiguration.RemoteDirectoryToDownload);
        var destination = new DirectoryPath(ExerciseConfiguration.DownloadDirectory);
        var authenticationProvider = new ClientSecretAuthenticationProvider(
            new Uri(ExerciseConfiguration.RelativityAddress), ExerciseConfiguration.Credentials);
        var token = CancellationTokenProvider.GetCancellationToken();
        
        var progressHandler = TransferProgressHandlerProvider.Get();
        // Try download directory without custom options, then try again to same directory
        // Later perform download with OverwritePolicy enabled to same directory
        // Later perform download with custom options to new directory
        var transferOptions = new DownloadDirectoryOptions()
        {
            //OverwritePolicy = true,
            //SkipTopLevelDirectory = true,
            //ExclusionPolicy = new NoTifFilesExclusionPolicy(),
            SkipTransferringEmptyDirectories = true,
            TransferRetryPolicyDefinition = TransferRetryPolicyDefinition.ExponentialPolicy(TimeSpan.FromMinutes(1), 3)
        };
        
        throw new NotImplementedException("Please provide way to download directory using Transfer SDK");
    }
}