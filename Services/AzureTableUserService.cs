using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using StudentTermTracker.Models;
using System.Diagnostics;

namespace StudentTermTracker.Services
{
    public interface IUserDataService
    {
        Task<User> GetUserAsync(string userId);
        Task CreateOrUpdateUserAsync(User user);
        Task DeleteUserAsync(string userId);
    }

    public class AzureTableUserService : IUserDataService
    {
        private readonly TableClient _tableClient;

        public AzureTableUserService(IConfiguration configuration)
        {

            string connectionString = AzureConfig.StorageConnectionString;
            string tableName = AzureConfig.TableName;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DB Connection String not properly configured...");
            }

            _tableClient = new TableClient(
                connectionString,
                tableName);

            // Create table if it doesn't exist. TODO: Should this be moved to startup? 
            _tableClient.CreateIfNotExists();
        }

        public async Task<User> GetUserAsync(string userId)
        {
            try
            {
                return await _tableClient.GetEntityAsync<User>("User", userId);
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                // Return null to signify user not found
                return null;
            }
        }

        public async Task CreateOrUpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            try
            {
                await _tableClient.UpsertEntityAsync(user);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating/updating user: {ex.Message}");
                throw;
            }
            
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _tableClient.DeleteEntityAsync("User", userId);
        }
    }
}
