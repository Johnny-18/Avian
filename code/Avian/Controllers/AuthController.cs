using Avian.Application.Queries;
using Avian.Application.Services;
using Avian.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avian.Controllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration configuration, IMediator mediator, IAuthService authService)
    {
        _configuration = configuration;
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Login([FromBody] LoginDto login, CancellationToken cancellationToken)
    {
        var request = new GetUserQuery
        {
            Email = login.Email,
            Password = login.Password,
        };

        var user = await _mediator.Send(request, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        var token = _authService.GenerateToken(user);
        return Ok(token);
    }
    
    public async Task<IActionResult> Register()
    {
        return Ok();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        return Ok();
    }
}