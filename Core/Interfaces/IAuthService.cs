using Cards.Core.DTOs;
using Cards.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Cards.Interfaces;

public interface IAuthService
{
    Task<AccessTokenResponse> RegisterAsync(UserDto input);
    Task<AccessTokenResponse> LoginAsync(UserDto input);
    string? UserId { get; }
    string? Role { get; }
}