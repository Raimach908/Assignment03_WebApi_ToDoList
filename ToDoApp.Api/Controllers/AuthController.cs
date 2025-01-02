using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Api.Data;
using ToDoApp.Api.Models;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace ToDoApp.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Signup Endpoint
        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] SignupModel signupModel)
        {
            if (string.IsNullOrEmpty(signupModel.Email) || string.IsNullOrEmpty(signupModel.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == signupModel.Email))
            {
                return BadRequest(new { message = "User with this email already exists." });
            }

            // Hash the password using BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(signupModel.Password);

            // Create a new user
            var user = new User
            {
                Username = signupModel.Username,
                Email = signupModel.Email,
                PasswordHash = hashedPassword
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        // Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { message = "Email and password are required." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginModel.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username ?? ""),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"] ?? "https://localhost:7099",
                Audience = _configuration["JwtSettings:Audience"] ?? "https://localhost:7099"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        // Get All Users Endpoint
        [HttpGet("users")]
        //[Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        // Get User by ID Endpoint
        [HttpGet("users/{id}")]
        //[Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }

        // Models for Signup and Login
        public class SignupModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
