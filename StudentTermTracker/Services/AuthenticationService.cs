using System.Net.Http.Json;
using System.Text.Json;
using StudentTermTracker.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace StudentTermTracker.Services
{
    public interface IAuthenticationService
    {
        UserAccountSession? UserAccountSession { get; }
        bool isAdmin { get; }
        Task FetchUserAccountSession();
        Task<bool> Authenticate(string username, string password);
        Task<bool> Register(string fullName, string userName, string password);
        void Logout();
    }
    

    public class AuthenticationService : IAuthenticationService
    {
        private const string USER_SESSION_STORAGE_KEY = "user_account_session";
        private readonly HttpClient _httpClient;
        private readonly CustomAuthStateProvider _authStateProvider;
        private UserAccountSession? _userAccountSession;

        public AuthenticationService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClientFactory.CreateClient("AppHttpClient");
            _authStateProvider = (CustomAuthStateProvider)authStateProvider;
            Console.WriteLine($"API Base URL: {_httpClient.BaseAddress}");
        }

        public UserAccountSession? UserAccountSession => _userAccountSession;

        public bool isAdmin => _userAccountSession is not null && 
            _userAccountSession.Role == "Admin";

        public async Task FetchUserAccountSession()
        {
            var userAccountSessionJson = await SecureStorage.Default.GetAsync(USER_SESSION_STORAGE_KEY);

            if (!string.IsNullOrWhiteSpace(userAccountSessionJson))
            {
                _userAccountSession = JsonSerializer.Deserialize<UserAccountSession>(userAccountSessionJson);
                _authStateProvider.UpdateAuthenticationState(_userAccountSession);
            }
        }
        
        public async Task<bool> Authenticate(string username, string password)
        {
            var result = false;

            try
            {
                Console.WriteLine($"Attempting to authenticate user: {username}");
                Console.WriteLine($"API Endpoint: {_httpClient.BaseAddress}api/Account/Login");
                
                var response = await _httpClient.PostAsJsonAsync("api/Account/Login", new 
                {
                    Username = username,
                    Password = password
                });

                Console.WriteLine($"Authentication Response Status: {response?.StatusCode}");
                
                if (response is not null && response.IsSuccessStatusCode)
                {
                    _userAccountSession = await response.Content.ReadFromJsonAsync<UserAccountSession>();
                    if (_userAccountSession is not null)
                    {
                        await SecureStorage.Default.SetAsync(USER_SESSION_STORAGE_KEY,
                            JsonSerializer.Serialize(_userAccountSession));
                        result = true;
                        _authStateProvider.UpdateAuthenticationState(_userAccountSession);
                        Console.WriteLine($"Successfully authenticated user: {username}");
                    }
                }
                else
                {
                    var errorContent = await response?.Content.ReadAsStringAsync();
                    Console.WriteLine($"Authentication failed. Error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return result;
        }

        public async Task<bool> Register(string fullName, string userName, string password)
        {
            try
            {
                Console.WriteLine($"Attempting to register user: {userName}");
                Console.WriteLine($"API Endpoint: {_httpClient.BaseAddress}api/Account/Register");
                
                var response = await _httpClient.PostAsJsonAsync("api/Account/Register", new 
                {
                    FullName = fullName,
                    UserName = userName,
                    Password = password,
                    Role = "User" // Default role for new users
                });

                Console.WriteLine($"Registration Response Status: {response?.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Registration failed. Error: {errorContent}");
                }
                else
                {
                    Console.WriteLine($"Successfully registered user: {userName}");
                }

                return response is not null && response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }

        public void Logout()
        {
            _userAccountSession = null;
            SecureStorage.Default.Remove(USER_SESSION_STORAGE_KEY);
            _authStateProvider.UpdateAuthenticationState(null);
            Console.WriteLine("User logged out");
        }
    }
}
