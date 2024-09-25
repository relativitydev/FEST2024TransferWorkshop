
namespace FEST24.Base;

public static class ExerciseConfiguration
{
    public static string RelativityAddress => "RELATIVITY_URL";
        
    public static OAuthCredentials Credentials => new("CLIENT_ID", "CLIENT_SECRET");

    public static string ClientName => "Relativity.Transfer.SDK.FEST.Workshop.Sample";

    private static string RelativityFileSharePath => @"RELATIVITY_FILESHARE";

    public static int WorkspaceId => 0;

    public static string DestinationRelativityFolderPath  => Path.Combine(RelativityFileSharePath, @"StructuredData\TransferSDK-Samples", WorkspaceId.ToString());

    public static string RemoteDirectoryToDownload => Path.Combine(DestinationRelativityFolderPath, "ImportSource");

    public static string DirectoryToSend => @"C:\DataSet\ImportSource";

    public static string DownloadDirectory => @"C:\DataSet\DownloadDirectory";

    public static string UploadItemsFilePath => @"C:\DataSet\Files.txt";

    public static string LoadFilePath => Path.Combine(DestinationRelativityFolderPath, "ImportExercise\\load_file_01.dat");
}