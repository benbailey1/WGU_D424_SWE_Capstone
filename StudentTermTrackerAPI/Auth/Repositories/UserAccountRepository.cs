using StudentTermTrackerAPI.Auth.Models;
using StudentTermTrackerAPI.Data;

namespace StudentTermTrackerAPI.Auth.Repositories
{
    public interface IUserAccountRepository
    {
        Task<List<UserAccount>> GetUserAccounts();
        Task<UserAccount?> GetUserAccountById(int id);
        Task CreateUserAccount(UserAccount userAccount);
        Task UpdateUserAccount(UserAccount userAccount);
        Task DeleteUserAccount(int id);
    }

    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly IDatabaseConnectionService _dbConnService;
        
        public UserAccountRepository(IDatabaseConnectionService dbConnService)
        {
            _dbConnService = dbConnService;
        }

        public async Task<List<UserAccount>> GetUserAccounts()
        {
            var results = await _dbConnService.QueryAsync<UserAccount>("SELECT * FROM UserAccounts");
            return results.ToList();
        }

        public async Task<UserAccount?> GetUserAccountById(int id)
        {
            var results = await _dbConnService.QuerySingleOrDefaultAsync<UserAccount>("SELECT * FROM UserAccounts WHERE Id = @Id", new { Id = id });
            return results;
        }

        public async Task CreateUserAccount(UserAccount userAccount)
        {
            await _dbConnService.ExecuteAsync("INSERT INTO UserAccounts (FullName, UserName, Password) VALUES (@FullName, @UserName, @Password, @Role)", userAccount);
        }

        public async Task UpdateUserAccount(UserAccount userAccount)
        {
            await _dbConnService.ExecuteAsync("UPDATE UserAccounts SET FullName = @FullName, UserName = @UserName, Password = @Password WHERE Id = @Id", userAccount);
        }

        public async Task DeleteUserAccount(int id)
        {
            await _dbConnService.ExecuteAsync("DELETE FROM UserAccounts WHERE Id = @Id", new { Id = id });
        }


    }
}
