using Avian.Application.Services;
using Avian.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace Avian.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Login([FromBody] LoginDto login, CancellationToken cancellationToken)
    {
        var user = await _userService.LoginAsync(login.Email, login.Password, cancellationToken);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var token = _authService.GenerateToken(user);
        return Ok(token);
    }
    
    [HttpPost("register")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Register([FromBody] CreateUserDto register, CancellationToken cancellationToken)
    {
        var user = await _userService.RegisterAsync(register.Email, register.Password, register.Role, cancellationToken);
        if (user is null)
        {
            return NotFound("User registered with the same email");
        }

        var token = _authService.GenerateToken(user);
        return Ok(token);
    }
}