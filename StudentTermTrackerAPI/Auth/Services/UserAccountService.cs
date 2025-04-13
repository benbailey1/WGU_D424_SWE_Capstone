using StudentTermTrackerAPI.Auth.Models;
using StudentTermTrackerAPI.Auth.Repositories;

namespace StudentTermTrackerAPI.Auth.Services
{
    public interface IUserAccountService
    {
        Task<List<UserAccount>> GetUserAccounts();
        Task<UserAccount?> GetUserAccountById(int id);
        Task CreateUserAccount(UserAccount userAccount);
        Task UpdateUserAccount(UserAccount userAccount);
        Task DeleteUserAccount(int id);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepo;
        
        public UserAccountService(IUserAccountRepository userAccountRepo)
        {
            _userAccountRepo = userAccountRepo;
        }

        public async Task<List<UserAccount>> GetUserAccounts()
        {
            return await _userAccountRepo.GetUserAccounts();
        }

        public async Task<UserAccount?> GetUserAccountById(int id)
        {
            return await _userAccountRepo.GetUserAccountById(id);
        }

        public async Task CreateUserAccount(UserAccount userAccount)
        {
            userAccount.Role = "User";
            await _userAccountRepo.CreateUserAccount(userAccount);
        }

        public async Task UpdateUserAccount(UserAccount userAccount)
        {
            await _userAccountRepo.UpdateUserAccount(userAccount);
        }

        public async Task DeleteUserAccount(int id)
        {
            await _userAccountRepo.DeleteUserAccount(id);
        }
    }
}
