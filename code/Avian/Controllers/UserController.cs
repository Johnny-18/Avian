using Avian.Application.Services;
using Avian.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avian.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/users")]
public sealed class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{email:required}")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync([FromRoute] string email, CancellationToken cancellationToken)
    {
        var user = await _userService.GetAsync(email, cancellationToken);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(UserDto.FromDomain(user));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(UsersDto), 200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        if (!users.Any())
        {
            return NoContent();
        }

        return Ok(new UsersDto
        {
            Users = users.Select(UserDto.FromDomain).ToArray(),
        });
    }
}