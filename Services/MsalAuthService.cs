using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MsalAuthService()
        {
            _pca = PublicClientApplicationBuilder
                .Create(AzureConfig.ClientId)
                .WithAuthority($"https://login.microsoftonline.com/{AzureConfig.TenantId}")
                .WithRedirectUri("msal1c1861e0-e669-4ecb-8c1e-0fb6fa932d3a://auth")
                .Build();
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

        public Task<string> GetUserIdAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsAuthenticatedAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> GetUserEmailAsync()
        {
            throw new NotImplementedException();
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
