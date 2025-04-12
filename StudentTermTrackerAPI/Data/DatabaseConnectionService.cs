using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace StudentTermTrackerAPI.Data
{
    public interface IDatabaseConnectionService
    {
        IDbConnection CreateConnection();
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null);
        Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null);
        Task<IDbTransaction> BeginTransactionAsync();
    }

    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private readonly string _connectionString;

        public DatabaseConnectionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
        {
            using var connection = CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<T>(sql, param, transaction);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, param, transaction);
        }

        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            var connection = CreateConnection();
            return connection.BeginTransaction();
        }
    }
} 