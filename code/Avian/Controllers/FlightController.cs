using System.ComponentModel.DataAnnotations;
using Avian.Application.Services;
using Avian.Dtos.Flight;
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

    [HttpGet("{id:guid:required}")]
    [ProducesResponseType(typeof(FlightDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var flight = await _flightService.GetFlightByIdAsync(id, cancellationToken);
        if (flight is null)
        {
            return NotFound();
        }
        
        return Ok(FlightDto.FromDomain(flight));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(FlightsDto), 200)]
    [ProducesResponseType(401)]
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

    [HttpPost]
    [ProducesResponseType(typeof(FlightDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> CreateAsync([FromBody][Required] CreateFlightDto flight, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _flightService.CreateAsync(
                flight.PlaneId,
                flight.Pilots,
                flight.Status,
                flight.DepartureDate,
                flight.ArrivalDate,
                flight.From,
                flight.To,
                cancellationToken);

            return Ok(FlightDto.FromDomain(created));
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("{id:guid:required}")]
    [ProducesResponseType(typeof(FlightDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<ActionResult> ChangeAsync([FromRoute] Guid id, [FromBody][Required] ChangeFlightDto flight, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _flightService.ChangeAsync(
                id,
                flight.Status,
                flight.Comment,
                cancellationToken);
            if (created is null)
            {
                return NotFound();
            }

            return Ok(FlightDto.FromDomain(created));
        }
        catch (ApplicationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id:guid:required}")]
    [ProducesResponseType(typeof(FlightDto), 200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var flight = await _flightService.DeleteAsync(
            id,
            cancellationToken);
        if (flight is null)
        {
            return NotFound();
        }

        return Ok(FlightDto.FromDomain(flight));
    }
}