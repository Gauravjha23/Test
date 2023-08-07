// Services/AuthService.cs
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class AuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> RegisterUserAsync(string username, string password)
    {
        var user = new User { UserName = username, Role = "User" };
        var result = await _userManager.CreateAsync(user, password);

        return result.Succeeded;
    }

    public async Task<bool> RegisterAdminAsync(string username, string password)
    {
        var user = new User { UserName = username, Role = "Admin" };
        var result = await _userManager.CreateAsync(user, password);

        return result.Succeeded;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            // Handle the null/empty username or password case and return an error message or throw an exception.
            // For example:
            throw new ArgumentException("Username and password are required.");
        }
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = _configuration["JwtSettings:Secret"];
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        return null;
    }
}
