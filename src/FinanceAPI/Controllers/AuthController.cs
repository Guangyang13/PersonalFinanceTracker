using Azure.Core;
using FinanceAPI.Data;
using FinanceAPI.Interfaces;
using FinanceAPI.Models;
using FinanceAPI.Models.Dtos;
using FinanceAPI.Models.Entity;
using FinanceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(ApiDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Username already exists");

            _authService.CreatePasswordHash(request.Password, out var passwordhash, out var passwordSalt);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordhash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User Registered");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !_authService.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid Credentials");

            var token = _authService.GenerateJwtToken(request.Username);
            return Ok(new { token });
        }


        [HttpPost("check-username")]
        public async Task<IActionResult> CheckUsername([FromBody] string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
                return BadRequest("Username already exists");

            return Ok("Username is available");
        }
    }
}
