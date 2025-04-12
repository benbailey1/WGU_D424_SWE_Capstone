using StudentTermTrackerAPI.Auth.Models;
using StudentTermTrackerAPI.Auth.Repositories;
using StudentTermTrackerAPI.Data;

namespace StudentTermTrackerAPI.Auth.Services
{
    public interface IUserAccountService
    {
        Task<List<UserAccount>> GetUserAccounts();
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly UserAccountRepository _userAccountRepo;
        
        public UserAccountService(UserAccountRepository userAccountRepo)
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
