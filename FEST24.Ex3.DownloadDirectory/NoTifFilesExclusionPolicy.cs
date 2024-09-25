using FEST24.Base;
using Relativity.Transfer.SDK.Interfaces.Options.Policies;

namespace FEST24.Workshop.Exercise3;

public class NoTifFilesExclusionPolicy : IFileExclusionPolicy
{
    public Task<bool> ShouldExcludeAsync(IFileReference fileReference)
    {
        if(Path.GetExtension(fileReference.AbsolutePath) == ".tif")
        {
            ConsoleLogger.PrintInformation($"Excluding {fileReference.AbsolutePath}");
            return Task.FromResult(true);
        }
        
        return Task.FromResult(false);
    }
}