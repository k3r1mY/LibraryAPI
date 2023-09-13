using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using LibraryAPI.dto;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static ErrorHandlingMiddleware;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly LibraryManagementSystemContext _context;
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config, LibraryManagementSystemContext dbContext)
        {
            _config = config;
            _context = dbContext;
        }

        /// <summary>
        /// Simulates login and generates a valid JWT
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(int memberId, string password)
        {
            Member member = _context.Members.FirstOrDefault(m => m.MemberId == memberId);

            // Check if the member is found and the provided password matches the stored password
            if (member == null)
            {
                throw new ApiException((int)HttpStatusCode.BadRequest, "MemberNotFound", "Member not found.");
            }

            if (!VerifyPassword(password, member.Password))
            {
                throw new ApiException((int)HttpStatusCode.Unauthorized, "InvalidPassword", "Authentication failed. Invalid Password.");
            }

            string token = GenerateJwtToken(member, memberId);
            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestModel model)
        {
            // Validate the registration data (you can add more validation as needed)
            if (model == null)
            {
                throw new ApiException((int)HttpStatusCode.BadRequest, "InvalidRequest", "Invalid registration request.");
            }

            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            {
                throw new ApiException((int)HttpStatusCode.BadRequest, "InvalidRequest", "Username and password are required.");
            }

            // Check if the username is already taken
            if (_context.Members.Any(m => m.FirstName == model.Username))
            {
                throw new ApiException((int)HttpStatusCode.BadRequest, "UsernameTaken", "Username is already taken.");
            }

            // Generate a random salt value
            string salt = GenerateRandomSalt();

            // Hash the user's password with the salt
            string hashedPassword = HashPassword(model.Password, salt);

            // Create a new Member entity
            Member newMember = new Member
            {
                FirstName = model.Username, 
                Password = hashedPassword,
                Salt = salt, // Store the salt in the database
                             // Add other properties as needed...
            };

            // Add the new member to the database
            _context.Members.Add(newMember);
            _context.SaveChanges();

            // Generate a JWT token for the newly registered member
            string token = GenerateJwtToken(newMember, newMember.MemberId);

            return Ok(new { Token = token });
        }

        // Generate a random salt
        private string GenerateRandomSalt()
        {
            var saltBytes = new byte[16]; // Adjust the size of the salt as needed
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // Hash the password with the salt
        private string HashPassword(string password, string salt)
        {
            string saltedPassword = password + salt;
            return BCrypt.Net.BCrypt.HashPassword(saltedPassword);
        }




        private string GenerateJwtToken(Member member, int memberId) // Update the parameter here
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, memberId.ToString()), // Convert memberId to string for the claim
                // Add more claims as needed, e.g., roles, permissions, etc.
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static readonly int KeySizeInBytes = 32; // 256 bits

        public static string GenerateRandomKey()
        {
            var key = new byte[KeySizeInBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
