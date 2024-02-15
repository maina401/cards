using Cards.Core.DTOs;
using Cards.Models;

namespace Cards.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(UserDto input);
    Task<string> LoginAsync(UserDto input);
}