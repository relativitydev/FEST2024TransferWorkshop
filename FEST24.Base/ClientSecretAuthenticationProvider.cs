using Relativity.Transfer.SDK.Interfaces.Authentication;

namespace FEST24.Base;

public class ClientSecretAuthenticationProvider : IRelativityAuthenticationProvider
{
    private string? _bearerToken;
    private readonly OAuthCredentials _credentials;
    private readonly BearerTokenProvider _bearerTokenProvider = new ();

    /// <summary>
    ///     This class represents an implementation of an Authentication Provider based on OAuth client id and client secret.
    ///     Be aware that this is sample implementation, and it should be used only for testing purposes.
    /// </summary>
    public ClientSecretAuthenticationProvider(Uri instanceUri,
        OAuthCredentials credentials)
    {
        _credentials = credentials;
        BaseAddress = instanceUri;
    }

    public Uri BaseAddress { get; }

    public async Task<RelativityCredentials> GetCredentialsAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_bearerToken))
        {
            //ConsoleLogger.PrintInformation("Authentication provider - Requesting credentials (CACHED)");

            return new RelativityCredentials(_bearerToken, BaseAddress);
        }

        ConsoleLogger.PrintInformation("Authentication provider - Requesting credentials");

        // The token is cached by TransferSDK, but it is important to note that a token may expire during long transfers.
        // Therefore, it is imperative that your implementation does not cache the token and always provides a valid one.
        _bearerToken = await _bearerTokenProvider.GetBearerTokenAsync(BaseAddress, _credentials).ConfigureAwait(false);

        return new RelativityCredentials(_bearerToken, BaseAddress);
    }
}