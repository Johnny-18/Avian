using Avian.Application.Services;
using Avian.Domain.Users;
using Avian.Dtos;
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
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Login([FromBody] LoginDto login, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(login.Email, login.Password, cancellationToken);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var token = _authService.GenerateToken(user);
        return Ok(token);
    }
}