// Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EmployeesApi.Data; // Replace with your DbContext namespace
using EmployeesApi.Models; // Replace with your User model namespace
using Microsoft.EntityFrameworkCore;
using EmployeesApi.Utility;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ApiDbContext _dbContext; // Add this field

    public AuthController(IConfiguration configuration, ApiDbContext dbContext)
    {
        _configuration = configuration;
        _dbContext = dbContext;
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await AuthenticateUserAsync(model.Username, model.Password);

        if (user != null)
        {
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        return Unauthorized(); // Invalid credentials
    }

    private async Task<User> AuthenticateUserAsync(string username, string password)
    {
        // Example: Authenticate user against a database
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == username);

        if (user == null || ! PasswordHash.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            return null; // User not found or password incorrect
        }

        return user;
    }


    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            // Add more claims as needed, e.g., user roles
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30), // Token expiration time
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

