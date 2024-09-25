using System.Dynamic;
using Newtonsoft.Json;

namespace FEST24.Base;

public class BearerTokenProvider
{
    private const string IdentityServiceTokenUri = "/Relativity/Identity/connect/token";
    
    public async Task<string?> GetBearerTokenAsync(Uri baseAddress, OAuthCredentials credentials)
    {
        throw new NotImplementedException("Get token is not implemented");
    }
}