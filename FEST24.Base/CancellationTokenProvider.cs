namespace FEST24.Base;

public class CancellationTokenProvider
{
    public CancellationToken GetCancellationToken()
    {
        using var cts = new CancellationTokenSource();
        
        HookCancellation(cts);

        return cts.Token;
    }

    private static void HookCancellation(CancellationTokenSource cts)
    {       
        void CancelEventHandler(object? sender, ConsoleCancelEventArgs cancelEventArgs)
        {
            if (cts.IsCancellationRequested)
            {
                return;
            }

            cts.Cancel();
            cancelEventArgs.Cancel = true;
            Console.CancelKeyPress -= CancelEventHandler;
            
            ConsoleLogger.PrintInformation("Terminating transfer...");
        }

        Console.CancelKeyPress += CancelEventHandler;
    }
}