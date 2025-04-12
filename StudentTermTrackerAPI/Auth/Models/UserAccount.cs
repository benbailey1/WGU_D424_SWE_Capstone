namespace StudentTermTrackerAPI.Auth.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
} 