using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using StudentTermTracker.Models;

namespace StudentTermTracker.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public void UpdateAuthenticationState(UserAccountSession? userSession)
        {
            if (userSession == null)
            {
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userSession.UserName),
                    new Claim(ClaimTypes.Role, userSession.Role)
                };

                var identity = new ClaimsIdentity(claims, "apiauth");
                _currentUser = new ClaimsPrincipal(identity);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
} 