using Cards.Core.DTOs;
using Cards.Interfaces;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Controllers;

/// <summary>
/// Controller for handling user authentication.
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <response code="200">Returns the newly registered user's JWT token.</response>
    /// <response code="400">If the registration fails.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]

    public async Task<IActionResult> Register(UserDto user)
    {
        var token = await authService.RegisterAsync(user);
        return Ok(token);
    }

    /// <summary>
    /// Authenticates a user and returns a JWT.
    /// </summary>
    /// <response code="200">Returns the authenticated user's JWT token.</response>
    /// <response code="400">If the login fails.</response>
    ///  <response code="422">If there's an error processing the request.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Login(UserDto user)
    {
        var token = await authService.LoginAsync(user);
        return Ok(token);
    }
}