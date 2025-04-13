using Microsoft.AspNetCore.Mvc;
using StudentTermTrackerAPI.Auth.Handlers;
using StudentTermTrackerAPI.Data;
using StudentTermTrackerAPI.Auth.Models;

namespace StudentTermTrackerAPI.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminSetupController : ControllerBase
    {
        private readonly IDatabaseConnectionService _db;
        private readonly IConfiguration _configuration;

        public AdminSetupController(IDatabaseConnectionService db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdminUser([FromBody] LoginRequestModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required");
            }

            // Check if any users exist
            var existingUsers = await _db.QueryAsync<UserAccount>("SELECT TOP 1 Id FROM UserAccounts");
            if (existingUsers.Any())
            {
                return BadRequest("Admin user can only be created when no users exist in the database");
            }

            var hashedPassword = PasswordHashHandler.HashPassword(model.Password);

            // Insert admin user
            var sql = @"
                INSERT INTO [dbo].[UserAccounts] ([FullName], [UserName], [Password], [Role])
                VALUES (@FullName, @UserName, @Password, @Role)";

            await _db.ExecuteAsync(sql, new
            {
                FullName = "System Administrator",
                UserName = model.UserName,
                Password = hashedPassword,
                Role = "Admin"
            });

            return Ok("Admin user created successfully");
        }
    }
} 