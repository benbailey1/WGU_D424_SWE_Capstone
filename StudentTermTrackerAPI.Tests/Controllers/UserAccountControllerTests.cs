using Microsoft.AspNetCore.Mvc;
using Moq;
using StudentTermTrackerAPI.Auth.Controllers;
using StudentTermTrackerAPI.Auth.Models;
using StudentTermTrackerAPI.Auth.Services;
using Xunit;

namespace StudentTermTrackerAPI.Tests.Controllers
{
    public class UserAccountControllerTests
    {
        private readonly Mock<IUserAccountService> _mockUserAccountService;
        private readonly UserAccountController _controller;

        public UserAccountControllerTests()
        {
            _mockUserAccountService = new Mock<IUserAccountService>();
            _controller = new UserAccountController(_mockUserAccountService.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllUserAccounts()
        {
            // Arrange
            var expectedAccounts = new List<UserAccount>
            {
                new UserAccount { Id = 1, UserName = "user1", FullName = "User One" },
                new UserAccount { Id = 2, UserName = "user2", FullName = "User Two" }
            };
            _mockUserAccountService.Setup(s => s.GetUserAccounts())
                .ReturnsAsync(expectedAccounts);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.Equal(expectedAccounts, result);
            _mockUserAccountService.Verify(s => s.GetUserAccounts(), Times.Once);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsUserAccount()
        {
            // Arrange
            var expectedAccount = new UserAccount { Id = 1, UserName = "user1", FullName = "User One" };
            _mockUserAccountService.Setup(s => s.GetUserAccountById(1))
                .ReturnsAsync(expectedAccount);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.Equal(expectedAccount, result);
            _mockUserAccountService.Verify(s => s.GetUserAccountById(1), Times.Once);
        }

        [Fact]
        public async Task Create_WithValidUserAccount_ReturnsCreatedAtAction()
        {
            // Arrange
            var userAccount = new UserAccount 
            { 
                UserName = "newuser", 
                FullName = "New User", 
                Password = "password123" 
            };
            _mockUserAccountService.Setup(s => s.CreateUserAccount(It.IsAny<UserAccount>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(userAccount);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(UserAccountController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(userAccount.Id, createdAtActionResult.RouteValues["id"]);
            _mockUserAccountService.Verify(s => s.CreateUserAccount(It.IsAny<UserAccount>()), Times.Once);
        }

        [Fact]
        public async Task Create_WithInvalidUserAccount_ReturnsBadRequest()
        {
            // Arrange
            var userAccount = new UserAccount 
            { 
                UserName = "", 
                FullName = "", 
                Password = "" 
            };

            // Act
            var result = await _controller.Create(userAccount);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            _mockUserAccountService.Verify(s => s.CreateUserAccount(It.IsAny<UserAccount>()), Times.Never);
        }

        [Fact]
        public async Task Delete_WithExistingId_ReturnsOk()
        {
            // Arrange
            var userAccount = new UserAccount { Id = 1, UserName = "user1", FullName = "User One" };
            _mockUserAccountService.Setup(s => s.GetUserAccountById(1))
                .ReturnsAsync(userAccount);
            _mockUserAccountService.Setup(s => s.DeleteUserAccount(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
            _mockUserAccountService.Verify(s => s.DeleteUserAccount(1), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockUserAccountService.Setup(s => s.GetUserAccountById(999))
                .ReturnsAsync((UserAccount)null);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockUserAccountService.Verify(s => s.DeleteUserAccount(It.IsAny<int>()), Times.Never);
        }
    }
} 