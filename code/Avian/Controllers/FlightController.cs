using System.ComponentModel.DataAnnotations;
using Avian.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avian.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/flights")]
public sealed class FlightController : ControllerBase
{
    private readonly IMediator _mediator;

    public FlightController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{flightId:guid:required}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get([FromRoute][Required] Guid flightId, CancellationToken cancellationToken)
    {
        var request = new GetFlightQuery
        {
            FlightId = flightId,
        };
        
        var flight = await _mediator.Send(request, cancellationToken);
        if (flight is null)
        {
            return NotFound();
        }
        
        return Ok(flight);
    }
}