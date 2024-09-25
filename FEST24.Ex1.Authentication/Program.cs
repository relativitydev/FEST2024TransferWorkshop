using FEST24.Base;

namespace FEST24.Authentication;

public static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            ConsoleLogger.PrintInformation("Authentication example");

            ConsoleLogger.PrintInformation(ExerciseConfiguration.RelativityAddress);

            var authProvider = new ClientSecretAuthenticationProvider(
                new Uri(ExerciseConfiguration.RelativityAddress), 
                ExerciseConfiguration.Credentials);

            var credentials = await authProvider.GetCredentialsAsync(CancellationToken.None);
            
            ConsoleLogger.PrintInformation("\nCredentials obtained");
            ConsoleLogger.PrintInformation($"TOKEN: {credentials.AccessToken} \n");
            ConsoleLogger.PrintInformation($"BASE ADDRESS: {credentials.BaseAddress} \n");
            
            ConsoleLogger.PrintInformation("Exercise Completed. Press any key to exit.");
            Console.ReadKey();
        }
        catch(Exception ex)
        {
            ConsoleLogger.PrintError(ex, "Authentication failed");
        }
    } 
}