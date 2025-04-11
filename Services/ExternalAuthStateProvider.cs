using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Security.Claims;

namespace StudentTermTracker.Services
{
    public class ExternalAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(new AuthenticationState(currentUser));

        public Task LogInAsync()
        {
            var loginTask = LoginAsyncCore();
            NotifyAuthenticationStateChanged(loginTask);

            return loginTask;

            async Task<AuthenticationState> LoginAsyncCore()
            {
                var user = await LoginWithExternalProviderAsync();
                currentUser = user;

                return new AuthenticationState(currentUser);
            }

        }

        private async Task<ClaimsPrincipal> LoginWithExternalProviderAsync()
        {
            var clientId = AzureConfig.ClientId;
            var tenantId = AzureConfig.TenantId;
            var redirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

            IPublicClientApplication publicClientApp = PublicClientApplicationBuilder
                .Create(clientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                .WithCacheOptions(new CacheOptions(true))
#if ANDROID
                .WithRedirectUri($"msal{clientId}://auth")
#elif IOS
        .WithRedirectUri($"msal{clientId}://auth")
#else
        // WithWindowsEmbeddedBrowserSupport required due to issue: https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/issues/4805
        // For some reason is shows exception, but the code builds fine.
        //.WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
        //.WithWindowsEmbeddedBrowserSupport()
#endif
        .Build();

            IAccount account = (await publicClientApp.GetAccountsAsync()).FirstOrDefault();
            AuthenticationResult result;
            var tryInteractiveLogin = false;

            try
            {
                result = await publicClientApp.AcquireTokenSilent(new string[] { "user.read" }, account).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                tryInteractiveLogin = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MSAL Silent Error: {ex.Message}");
            }

            if (tryInteractiveLogin)
            {
                try
                {
                    result = await publicClientApp.AcquireTokenInteractive(new string[] { "user.read" }).ExecuteAsync();
                    account = (await publicClientApp.GetAccountsAsync()).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"MSAL Interactive Error: {ex.Message}");
                }
            }

            var authState = await GetAuthenticationStateAsync();

            var user = account.GetTenantProfiles().First().ClaimsPrincipal;

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(user.Claims, "Bearer"));

            return authenticatedUser;
        }

        public async Task LogoutAsync()
        {
            currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(currentUser)));
        }
    }
}
