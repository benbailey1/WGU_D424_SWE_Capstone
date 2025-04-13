using System.Net.Http.Json;
using System.Text.Json;
using StudentTermTracker.Models;

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
        private UserAccountSession? _userAccountSession;

        public AuthenticationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AppHttpClient");
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
            }
        }
        
        public async Task<bool> Authenticate(string username, string password)
        {
            var result = false;

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Account/Login", new 
                {
                    Username = username,
                    Password = password
                });

                if (response is not null && response.IsSuccessStatusCode)
                {
                    _userAccountSession = await response.Content.ReadFromJsonAsync<UserAccountSession>();
                    if (_userAccountSession is not null)
                    {
                        await SecureStorage.Default.SetAsync(USER_SESSION_STORAGE_KEY,
                            JsonSerializer.Serialize(_userAccountSession));
                        result = true;
                    }
                }
                
               // return result;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error authenticating: {ex.Message}");
            }

            return result;
        }

        public async Task<bool> Register(string fullName, string userName, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Account/Register", new 
            {
                FullName = fullName,
                UserName = userName,
                Password = password,
                Role = "User" // Default role for new users
            });

            return response is not null && response.IsSuccessStatusCode;
        }

        public void Logout()
        {
            _userAccountSession = null;
            SecureStorage.Default.Remove(USER_SESSION_STORAGE_KEY);
        }

    }
}
