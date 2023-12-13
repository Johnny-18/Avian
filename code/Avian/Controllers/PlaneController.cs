using Avian.Application.Services;
using Avian.Dtos.Plane;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avian.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/planes")]
public sealed class PlaneController : ControllerBase
{
    private readonly IPlaneService _planeService;

    public PlaneController(IPlaneService planeService)
    {
        _planeService = planeService;
    }

    [HttpGet("{id:guid:required}")]
    [ProducesResponseType(typeof(PlaneDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var plane = await _planeService.GetAsync(id, cancellationToken);
        if (plane is null)
        {
            return NotFound();
        }

        return Ok(PlaneDto.FromDomain(plane));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(PlanesDto), 200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var planes = await _planeService.GetAllAsync(cancellationToken);
        if (!planes.Any())
        {
            return NoContent();
        }

        return Ok(new PlanesDto
        {
            Planes = planes.Select(PlaneDto.FromDomain).ToArray(),
        });
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(PlaneDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePlaneDto plane, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(plane.Name))
        {
            return BadRequest();
        }
        
        var created = await _planeService.CreateAsync(plane.Name, plane.Status, cancellationToken);

        return Ok(PlaneDto.FromDomain(created));
    }
    
    [HttpDelete("{id:guid:required}")]
    [ProducesResponseType(typeof(PlaneDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var created = await _planeService.DeleteAsync(id, cancellationToken);
        if (created is null)
        {
            return NotFound();
        }

        return Ok(PlaneDto.FromDomain(created));
    }
}