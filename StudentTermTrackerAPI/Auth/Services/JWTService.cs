using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using StudentTermTrackerAPI.Auth.Handlers;
using StudentTermTrackerAPI.Auth.Models;
using StudentTermTrackerAPI.Data;

namespace StudentTermTrackerAPI.Auth.Services
{
    public class JWTService
    {
        private readonly IDatabaseConnectionService _db;
        private readonly IConfiguration _configuration;
        
        public JWTService(IDatabaseConnectionService db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<LoginResponseModel?> Authenticate(LoginRequestModel model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                return null;
            }

            // Use Dapper to query the user account
            var userAccount = await _db.QuerySingleOrDefaultAsync<UserAccount>(
                "SELECT * FROM UserAccounts WHERE UserName = @UserName",
                new { UserName = model.UserName }
            );

            if (userAccount is null || !PasswordHashHandler.VerifyPassword(model.Password, userAccount.Password!))
            {
                return null;
            }

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:Key"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, model.UserName),
                    new Claim(ClaimTypes.Role, userAccount.Role!)
                }),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key!)),
                    SecurityAlgorithms.HmacSha512Signature
                ),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseModel
            {
                AccessToken = accessToken,
                UserName = model.UserName,
                Role = userAccount.Role
            };
        }
    }
}
