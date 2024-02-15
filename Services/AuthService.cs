using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cards.Core.DTOs;
using Cards.Enums;
using Cards.Interfaces;
using Cards.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cards.Services;

public class AuthService(UserManager<User> userManager, IOptionsSnapshot<JwtSettings> jwtSettings)
    : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<string> RegisterAsync(UserDto input)
    {
        var user = new User
        {
            Email = input.Email,
            UserName = input.Email,
            Role = new Role { AccessLevel = AccessLevel.Member }
        };
        var result = await userManager.CreateAsync(user, input.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Registration failed");
        }

        return GenerateJwtToken(user);
    }

    public async Task<string> LoginAsync(UserDto input)
    {
        var user = await userManager.FindByEmailAsync(input.Email);

        if (user == null || !await userManager.CheckPasswordAsync(user, input.Password))
        {
            throw new InvalidOperationException("Invalid login attempt");
        }

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}