using System.ComponentModel.DataAnnotations;
using Avian.Application.Services;
using Avian.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avian.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/flights")]
public sealed class FlightController : ControllerBase
{
    private readonly IFlightService _flightService;
    
    public FlightController(IFlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet("{flightId:guid:required}")]
    [ProducesResponseType(typeof(FlightDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync([FromRoute][Required] Guid flightId, CancellationToken cancellationToken)
    {
        var flight = await _flightService.GetFlightByIdAsync(flightId, cancellationToken);
        if (flight is null)
        {
            return NotFound();
        }
        
        return Ok(FlightDto.FromDomain(flight));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(FlightsDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var flights = await _flightService.GetFlightsAsync(cancellationToken);
        if (!flights.Any())
        {
            return NoContent();
        }

        var response = new FlightsDto
        {
            Flights = flights.Select(FlightDto.FromDomain).ToArray(),
        };

        return Ok(response);
    }
}