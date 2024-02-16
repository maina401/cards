using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cards.Core.DTOs;
using Cards.Enums;
using Cards.Interfaces;
using Cards.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cards.Services;

public class AuthService(
    UserManager<User> userManager,
    IOptionsSnapshot<JwtSettings> jwtSettings,
    IHttpContextAccessor httpContextAccessor,
    AppDbContext appDbContext)
    : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;


    public string? UserId => httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

    public string? Role => httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;


    public async Task<AccessTokenResponse> RegisterAsync(UserDto input)
    {
        var user = new User
        {
            Email = input.Email,
            UserName = input.Email,
            Role = new Role { AccessLevel = AccessLevel.Member }
        };
        var result = await userManager.CreateAsync(user, input.Password);

        if (result.Succeeded)
            return new AccessTokenResponse()
            {
                AccessToken = GenerateJwtToken(user),
                ExpiresIn = (long)_jwtSettings.TokenLifetime.TotalSeconds,
                RefreshToken = GenerateJwtToken(user)
            };
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException("Registration failed: " + errors);

    }

    public async Task<AccessTokenResponse> LoginAsync(UserDto input)
    {
        var user = await appDbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == input.Email);

        if (user == null || !await userManager.CheckPasswordAsync(user, input.Password))
        {
            throw new InvalidOperationException("Invalid login attempt");
        }

        return new AccessTokenResponse()
        {
            AccessToken = GenerateJwtToken(user),
            ExpiresIn = (long)_jwtSettings.TokenLifetime.TotalSeconds,
            RefreshToken = GenerateJwtToken(user)
        };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.AccessLevel.ToString())
                }
                ),
            Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}