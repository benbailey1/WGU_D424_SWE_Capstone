using Microsoft.Extensions.Configuration;
using Moq;
using StudentTermTrackerAPI.Auth.Handlers;
using StudentTermTrackerAPI.Auth.Models;
using StudentTermTrackerAPI.Auth.Services;
using StudentTermTrackerAPI.Data;

namespace StudentTermTracker.Tests.ApiTests
{
    [TestClass]
    public class JWTServiceTests
    {
        private Mock<IDatabaseConnectionService> _mockDbService = null!;
        private Mock<IConfiguration> _mockConfiguration = null!;
        private JWTService _jwtService = null!;

        [TestInitialize]
        public void Initialize()
        {
            _mockDbService = new Mock<IDatabaseConnectionService>();
            _mockConfiguration = new Mock<IConfiguration>();
            
            // Setup configuration mock
            var jwtConfigSection = new Mock<IConfigurationSection>();
            jwtConfigSection.Setup(x => x["Issuer"]).Returns("http://localhost:5225/");
            jwtConfigSection.Setup(x => x["Audience"]).Returns("http://localhost:5225/");
            jwtConfigSection.Setup(x => x["Key"]).Returns("test-key-123456789012345678901234567890");

            _mockConfiguration.Setup(x => x["JwtConfig:Issuer"]).Returns("http://localhost:5225/");
            _mockConfiguration.Setup(x => x["JwtConfig:Audience"]).Returns("http://localhost:5225/");
            _mockConfiguration.Setup(x => x["JwtConfig:Key"]).Returns("test-key-12345678901234567890");
            
            _jwtService = new JWTService(_mockDbService.Object, _mockConfiguration.Object);
        }

        [TestMethod]
        public async Task Authenticate_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            var loginRequest = new LoginRequestModel
            {
                UserName = "invaliduser",
                Password = "wrongpassword"
            };

            _mockDbService.Setup(db => db.QuerySingleOrDefaultAsync<UserAccount>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null))
                .ReturnsAsync((UserAccount)null!);

            // Act
            var result = await _jwtService.Authenticate(loginRequest);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Authenticate_WithValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginRequest = new LoginRequestModel
            {
                UserName = "validuser",
                Password = "correctpassword"
            };

            var hashedPassword = PasswordHashHandler.HashPassword("correctpassword");
            var userAccount = new UserAccount
            {
                Id = 1,
                UserName = "validuser",
                Password = hashedPassword,
                Role = "User"
            };

            _mockDbService.Setup(db => db.QuerySingleOrDefaultAsync<UserAccount>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null))
                .ReturnsAsync(userAccount);

            // Act
            var result = await _jwtService.Authenticate(loginRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("validuser", result.UserName);
            Assert.AreEqual("User", result.Role);
            Assert.IsNotNull(result.AccessToken);
        }

        [TestMethod]
        public async Task Authenticate_WithEmptyCredentials_ReturnsNull()
        {
            // Arrange
            var loginRequest = new LoginRequestModel
            {
                UserName = "",
                Password = ""
            };

            // Act
            var result = await _jwtService.Authenticate(loginRequest);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Authenticate_WithNullCredentials_ReturnsNull()
        {
            // Arrange
            var loginRequest = new LoginRequestModel
            {
                UserName = null,
                Password = null
            };

            // Act
            var result = await _jwtService.Authenticate(loginRequest);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Authenticate_WithWrongPassword_ReturnsNull()
        {
            // Arrange
            var loginRequest = new LoginRequestModel
            {
                UserName = "validuser",
                Password = "wrongpassword"
            };

            var hashedPassword = PasswordHashHandler.HashPassword("correctpassword");
            var userAccount = new UserAccount
            {
                Id = 1,
                UserName = "validuser",
                Password = hashedPassword,
                Role = "User"
            };

            _mockDbService.Setup(db => db.QuerySingleOrDefaultAsync<UserAccount>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null))
                .ReturnsAsync(userAccount);

            // Act
            var result = await _jwtService.Authenticate(loginRequest);

            // Assert
            Assert.IsNull(result);
        }
    }
} 