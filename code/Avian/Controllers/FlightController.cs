using System.ComponentModel.DataAnnotations;
using Avian.Application.Services;
using Avian.Domain.ValueObjects;
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

    [HttpGet("{flightId:guid:required}")]
    [ProducesResponseType(typeof(FlightDto), 200)]
    [ProducesResponseType(401)]
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
    public async Task<ActionResult> CreateAsync([FromBody] CreateFlightDto flight, CancellationToken cancellationToken)
    {
        if (flight.Status is FlightStatuses.Canceled or FlightStatuses.Completed)
        {
            return BadRequest("Invalid status for flight!");
        }

        if (flight.ArrivalDate is not null && flight.DepartureDate > flight.ArrivalDate)
        {
            return BadRequest("Invalid dates!");
        }

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
}