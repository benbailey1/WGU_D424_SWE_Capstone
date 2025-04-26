using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentTermTrackerAPI.Auth.Models;
using StudentTermTrackerAPI.Auth.Repositories;
using StudentTermTrackerAPI.Auth.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentTermTracker.Tests.ApiTests
{
    [TestClass]
    public class UserAccountServiceTests
    {
        private Mock<IUserAccountRepository> _mockUserAccountRepo = null!;
        private UserAccountService _userAccountService = null!;

        [TestInitialize]
        public void Initialize()
        {
            _mockUserAccountRepo = new Mock<IUserAccountRepository>();
            _userAccountService = new UserAccountService(_mockUserAccountRepo.Object);
        }

        [TestMethod]
        public async Task GetUserAccounts_ReturnsListOfUserAccounts()
        {
            // Arrange
            var expectedUserAccounts = new List<UserAccount>
            {
                new UserAccount { Id = 1, FullName = "User One", UserName = "user1", Role = "User" },
                new UserAccount { Id = 2, FullName = "User Two", UserName = "user2", Role = "User" }
            };

            _mockUserAccountRepo.Setup(repo => repo.GetUserAccounts())
                .ReturnsAsync(expectedUserAccounts);

            // Act
            var result = await _userAccountService.GetUserAccounts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("user1", result[0].UserName);
            Assert.AreEqual("user2", result[1].UserName);
        }

        [TestMethod]
        public async Task GetUserAccountById_ReturnsUserAccount()
        {
            // Arrange
            var expectedUserAccount = new UserAccount { Id = 1, FullName = "Test User", UserName = "testuser", Role = "User" };
            _mockUserAccountRepo.Setup(repo => repo.GetUserAccountById(1))
                .ReturnsAsync(expectedUserAccount);

            // Act
            var result = await _userAccountService.GetUserAccountById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("testuser", result.UserName);
        }

        [TestMethod]
        public async Task CreateUserAccount_SetsDefaultRoleAndCallsRepository()
        {
            // Arrange
            var newUserAccount = new UserAccount { FullName = "New User", UserName = "newuser" };
            _mockUserAccountRepo.Setup(repo => repo.CreateUserAccount(It.IsAny<UserAccount>()))
                .Returns(Task.CompletedTask);

            // Act
            await _userAccountService.CreateUserAccount(newUserAccount);

            // Assert
            Assert.AreEqual("User", newUserAccount.Role);
            _mockUserAccountRepo.Verify(repo => repo.CreateUserAccount(newUserAccount), Times.Once);
        }

        [TestMethod]
        public async Task UpdateUserAccount_CallsRepository()
        {
            // Arrange
            var userAccount = new UserAccount { Id = 1, FullName = "Updated User", UserName = "updateduser", Role = "User" };
            _mockUserAccountRepo.Setup(repo => repo.UpdateUserAccount(It.IsAny<UserAccount>()))
                .Returns(Task.CompletedTask);

            // Act
            await _userAccountService.UpdateUserAccount(userAccount);

            // Assert
            _mockUserAccountRepo.Verify(repo => repo.UpdateUserAccount(userAccount), Times.Once);
        }

        [TestMethod]
        public async Task DeleteUserAccount_CallsRepository()
        {
            // Arrange
            int userId = 1;
            _mockUserAccountRepo.Setup(repo => repo.DeleteUserAccount(userId))
                .Returns(Task.CompletedTask);

            // Act
            await _userAccountService.DeleteUserAccount(userId);

            // Assert
            _mockUserAccountRepo.Verify(repo => repo.DeleteUserAccount(userId), Times.Once);
        }
    }
} 