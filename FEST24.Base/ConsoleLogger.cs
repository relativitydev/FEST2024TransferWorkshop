using ByteSizeLib;
using Relativity.Transfer.SDK.Interfaces.ProgressReporting;
using Relativity.Transfer.SDK.Interfaces.Paths;

namespace FEST24.Base;

public static class ConsoleLogger
{
    public static void PrintCreatingTransfer(Guid jobId, PathBase source, PathBase destination,
        params string[] additionalLines)
    {
        Console.WriteLine();
        Console.WriteLine($"Creating transfer {jobId}:");

        if (source != null) Console.WriteLine($"\tFrom: {source}");
        if (destination != null) Console.WriteLine($"\tTo: {destination}");

        AddAdditionalLinesIfRequired(additionalLines);

        Console.WriteLine();
    }

    public static void PrintTransferResult(TransferJobResult result, string headerLine = "Transfer has finished:",
        bool waitForKeyHit = true)
    {
        Console.WriteLine();
        Console.WriteLine(headerLine);
        Console.WriteLine($" JobId: {result.CorrelationId}");
        Console.WriteLine(
            $" Total Bytes: {result.TotalBytes} ({ByteSize.FromBytes(result.TotalBytes)})");
        Console.WriteLine($" Total Files Transferred: {result.TotalFilesTransferred}");
        Console.WriteLine(
            $" Total Empty Directories Transferred: {result.TotalEmptyDirectoriesTransferred}");
        Console.WriteLine($" Total Files Skipped: {result.TotalFilesSkipped}");
        Console.WriteLine($" Total Files Failed: {result.TotalFilesFailed}");
        Console.WriteLine($" Total Empty Directories Failed: {result.TotalEmptyDirectoriesFailed}");
        Console.WriteLine(
            $" Elapsed: {result.Elapsed:hh\\:mm\\:ss} s ({Math.Floor(result.Elapsed.TotalSeconds)} s)");
        Console.WriteLine($" State: {result.State.Status}");
        Console.WriteLine();

        AddProgressStepsDescription(result);

        if (!waitForKeyHit) return;
    }

    public static void PrintError(Exception ex, string? headerLine = null)
    {
        Console.WriteLine();
        Console.WriteLine(headerLine ?? "An error occurred:");
        Console.WriteLine(ex.Message);
        Console.WriteLine();
    }

    public static void PrintInformation(string information)
    {
        Console.WriteLine(information);
    }

    private static void AddAdditionalLinesIfRequired(string[] additionalLines)
    {
        var lines = (additionalLines ?? Array.Empty<string>()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        if (lines.Length == 0) return;

        Console.WriteLine();
        foreach (var additionalLine in lines)
            Console.WriteLine(additionalLine);
    }

    private static void AddProgressStepsDescription(TransferJobResult result)
    {
        if (!result.ProgressSteps.Any()) return;

        Console.WriteLine("  Job's steps:");
        foreach (var stepProgress in result.ProgressSteps)
            Console.WriteLine(
                $"    Step {stepProgress.StepType}: {stepProgress.PercentageProgress:F}%, {stepProgress.State}");
        Console.WriteLine();
    }

    public static void PrintRegisteringTransferJob(Guid jobId, DirectoryPath destination)
    {
        Console.WriteLine();
        Console.WriteLine($"Job registration {jobId}");
        Console.WriteLine($"Destination path {destination}");
        Console.WriteLine();
    }

    public static void PrintColoredInformation(string message, ConsoleColor color)
    {
        var saveColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = saveColor;
    }
}