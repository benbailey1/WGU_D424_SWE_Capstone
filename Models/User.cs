using Azure;
using Azure.Data.Tables;

namespace StudentTermTracker.Models
{
    public class User : ITableEntity
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedAt { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public User()
        {
            // Default constructor req'd for Table binding
        }

        public User(string userId, string email, string displayName)
        {
            Id = userId ?? throw new ArgumentNullException(nameof(userId));
            Email = email ?? "unknown@email.com";
            DisplayName = displayName ?? "Unknown User";
            CreatedAt = DateTime.UtcNow;

            // Set partition key and row key for Table storage
            PartitionKey = "User";
            RowKey = userId;
        }
    }
}
