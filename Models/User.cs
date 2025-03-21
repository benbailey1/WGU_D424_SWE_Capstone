using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentTermTracker.Models
{
    public class User : ITableEntity
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public DateTime CreatedAt { get; set; }

        public string PartitionKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RowKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? Timestamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ETag ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public User()
        {
            // Default constructor req'd for Table binding
        }

        public User(string userId, string email, string displayName)
        {
            Id = userId;
            Email = email;
            DisplayName = displayName;
            CreatedAt = DateTime.UtcNow;

            // Set partition key and row key for Table storage
            PartitionKey = "User";
            RowKey = userId;
        }
    }
}
