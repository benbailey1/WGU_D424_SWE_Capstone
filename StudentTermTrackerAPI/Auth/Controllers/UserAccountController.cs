using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentTermTrackerAPI.Auth.Handlers;
using StudentTermTrackerAPI.Auth.Models;
using StudentTermTrackerAPI.Auth.Services;
using StudentTermTrackerAPI.Data;

namespace StudentTermTrackerAPI.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAccountController : ControllerBase
    {
        private readonly UserAccountService _userAccountService;

        public UserAccountController(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }
        
        // GET: api/<UserAccountController>
        [Authorize]
        [HttpGet]
        public async Task<List<UserAccount>> Get()
        {
            return await _userAccountService.GetUserAccounts();
        }

        // GET api/<UserAccountController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<UserAccount?> GetById(int id)
        {
            return await _userAccountService.GetUserAccountById(id);    
        }

        // POST api/<UserAccountController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] UserAccount userAccount)
        {
            if(string.IsNullOrEmpty(userAccount.FullName) ||
                string.IsNullOrEmpty(userAccount.UserName) ||
                string.IsNullOrEmpty(userAccount.Password))
            {
                return BadRequest("Full Name, User Name, and Password are required");
            }

            userAccount.Password = PasswordHashHandler.HashPassword(userAccount.Password);
            await _userAccountService.CreateUserAccount(userAccount);

            return CreatedAtAction(nameof(GetById), new { id = userAccount.Id }, userAccount);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Update([FromBody] UserAccount userAccount)
        {
            if (userAccount.Id == 0 ||
                string.IsNullOrWhiteSpace(userAccount.FullName) ||
                string.IsNullOrWhiteSpace(userAccount.UserName) ||
                string.IsNullOrWhiteSpace(userAccount.Password))
            {
                return BadRequest("Invalid Request");
            }

            userAccount.Password = PasswordHashHandler.HashPassword(userAccount.Password);
            await _userAccountService.UpdateUserAccount(userAccount);

            return Ok();

        } 

        // PUT api/<UserAccountController>/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }

        // DELETE api/<UserAccountController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userAccount = await _userAccountService.GetUserAccountById(id);
            if (userAccount is null)
                return NotFound();

            await _userAccountService.DeleteUserAccount(id);
            return Ok();
        }
    }
}
