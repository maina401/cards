using Cards.Core.DTOs;
using Cards.Interfaces;
using Cards.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto user)
    {
        var token = await authService.RegisterAsync(user);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto user)
    {
        var token = await authService.LoginAsync(user);
        return Ok(token);
    }
}