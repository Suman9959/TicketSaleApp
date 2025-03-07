using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSaleAPI.DTOs;
using TicketSaleAPI.Services;

namespace TicketSaleAPI.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPost("tickets")]
    public async Task<ActionResult<TicketResponseDto>> BookTicket([FromBody] TicketCreateDto ticketDto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User not authenticated"));
        var ticket = await _ticketService.BookTicketAsync(userId, ticketDto);
        return CreatedAtAction(nameof(GetUserTickets), ticket);
    }

    [HttpGet("my-tickets")]
    public async Task<ActionResult<IEnumerable<TicketResponseDto>>> GetUserTickets()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User not authenticated"));
        var tickets = await _ticketService.GetUserTicketsAsync(userId);
        return Ok(tickets);
    }

    [HttpDelete("tickets/{id}")]
    public async Task<ActionResult> CancelTicket(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User not authenticated"));
        await _ticketService.CancelTicketAsync(id, userId);
        return NoContent();
    }
}