using Relativity.Transfer.SDK.Core.ProgressReporting;
using Relativity.Transfer.SDK.Interfaces.ProgressReporting;

namespace FEST24.Base;

public static class TransferProgressHandlerProvider
{
    public static ITransferProgressHandler Get()
    {
        var progressHandler = TransferProgressHandlerBuilder.Instance
            .OnStatistics(PrintStatistics)
            .OnSucceededItem(PrintSucceededItem)
            .OnFailedItem(PrintFailedItem)
            .OnSkippedItem(PrintSkippedItem)
            .OnProgressSteps(PrintProgressStep)
            .Create();

        return progressHandler;
    }

    private static void PrintStatistics(TransferJobStatistics statistics)
    {
        ConsoleLogger.PrintInformation($"  bytes transferred: {statistics.CurrentBytesTransferred} of {statistics.TotalBytes}");
    }

    private static void PrintSucceededItem(TransferItemState itemState)
    {
        ConsoleLogger.PrintInformation($"  item transfer succeeded: {itemState.Source}");
    }

    private static void PrintFailedItem(TransferItemState itemState)
    {
        ConsoleLogger.PrintInformation($"  item transfer failed: {itemState.Source}");
    }

    private static void PrintSkippedItem(TransferItemState itemState)
    {
        ConsoleLogger.PrintInformation($"  item transfer skipped: {itemState.Source}");
    }

    private static void PrintProgressStep(IEnumerable<StepProgress> progressSteps)
    {
        ConsoleLogger.PrintInformation(
            string.Join(Environment.NewLine,
                progressSteps.Select(stepProgress =>
                    $"    step name: {stepProgress.StepType}, {stepProgress.PercentageProgress:F}%, state: {stepProgress.State}")));
    }
}