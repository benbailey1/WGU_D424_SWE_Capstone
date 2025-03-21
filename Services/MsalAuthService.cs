using Microsoft.Identity.Client;
using System.Diagnostics;
#if ANDROID
using Android.App;
#endif

namespace StudentTermTracker.Services
{
    public interface IAuthService
    {
        Task<AuthenticationResult> SignInAsync();
        Task SignOutAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<string> GetUserEmailAsync();
        Task<string> GetUserIdAsync();
        Task<string> GetUserNameAsync();
        
    }
    public class MsalAuthService : IAuthService
    {
        private readonly IPublicClientApplication _pca;
        private readonly string[] _scopes = { "User.Read" };
        private IAccount _account;

#if ANDROID
        private Android.App.Activity _currentActivity;
#endif

#if ANDROID
        public MsalAuthService(Android.App.Activity currentActivity = null)
#else
        public MsalAuthService()
#endif
        {
#if ANDROID
        _currentActivity = currentActivity;
#endif

            var builder = PublicClientApplicationBuilder
                .Create(AzureConfig.ClientId)
                .WithAuthority($"https://login.microsoftonline.com/{AzureConfig.TenantId}")
                .WithRedirectUri($"msal{AzureConfig.ClientId}://auth");

#if ANDROID
        if (_currentActivity != null)
        {
            builder = builder.WithParentActivityOrWindow(() => _currentActivity);
        }
#endif

            _pca = builder.Build();
        }
        public async Task<AuthenticationResult> SignInAsync()
        {
            try
            {
                // try to get account silently first
                var accounts = await _pca.GetAccountsAsync();
                _account = accounts.FirstOrDefault();

                if (_account != null)
                {
                    return await _pca.AcquireTokenSilent(_scopes, _account).ExecuteAsync();
                }

                var result = await _pca.AcquireTokenInteractive(_scopes)
                    .WithUseEmbeddedWebView(true)
                    .ExecuteAsync();

                _account = result.Account;
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Authentication error: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetUserIdAsync()
        {
            var result = await SignInAsync();
            return result.UniqueId;
        }

        public async Task<string> GetUserNameAsync()
        {
            var result = await SignInAsync();
            return result.ClaimsPrincipal.FindFirst("name")?.Value ?? "Unknown User";
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var accounts = await _pca.GetAccountsAsync();
            return accounts.Any();
        }

        public async Task<string> GetUserEmailAsync()
        {
            var result = await SignInAsync();
            return result.ClaimsPrincipal.FindFirst("preferred_username")?.Value ?? result.Account?.Username ?? "unknown@email.com";
        }

        public async Task SignOutAsync()
        {
            var accounts = await _pca.GetAccountsAsync();

            foreach (var account in accounts)
            {
                await _pca.RemoveAsync(account);
            }

            _account = null;
        }

    }
}
