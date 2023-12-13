using Avian.Application.Services;
using Avian.Dtos;
using Avian.Dtos.Pilot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avian.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/pilots")]
public sealed class PilotController : ControllerBase
{
    private readonly IPilotService _pilotService;

    public PilotController(IPilotService pilotService)
    {
        _pilotService = pilotService;
    }

    [HttpGet("{id:guid:required}")]
    [ProducesResponseType(typeof(PilotDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var pilot = await _pilotService.GetAsync(id, cancellationToken);
        if (pilot is null)
        {
            return NotFound();
        }

        return Ok(PilotDto.FromDomain(pilot));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PilotsDto), 200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var pilots = await _pilotService.GetAllAsync(cancellationToken);
        if (!pilots.Any())
        {
            return NoContent();
        }

        return Ok(new PilotsDto
        {
            Pilots = pilots.Select(PilotDto.FromDomain).ToArray(),
        });
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(PilotDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePilotDto pilot, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(pilot.Name) || string.IsNullOrWhiteSpace(pilot.Qualification))
        {
            return BadRequest();
        }
        
        var created = await _pilotService.CreateAsync(pilot.Name, pilot.Qualification, cancellationToken);

        return Ok(PilotDto.FromDomain(created));
    }
    
    [HttpDelete("{id:guid:required}")]
    [ProducesResponseType(typeof(PilotDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var created = await _pilotService.DeleteAsync(id, cancellationToken);
        if (created is null)
        {
            return NotFound();
        }

        return Ok(PilotDto.FromDomain(created));
    }
}