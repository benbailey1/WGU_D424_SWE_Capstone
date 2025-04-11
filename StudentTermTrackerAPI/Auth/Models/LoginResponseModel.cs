namespace StudentTermTrackerAPI.Auth.Models
{
    public class LoginResponseModel
    {
        public string? UserName { get; set; }
        public string? AccessToken { get; set; }
        public string? Role { get; set; }
    }
}
