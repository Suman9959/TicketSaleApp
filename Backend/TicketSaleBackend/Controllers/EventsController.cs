using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSaleAPI.DTOs;
using TicketSaleAPI.Services;

namespace TicketSaleAPI.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAllEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponseDto>> GetEventById(Guid id)
    {
        var @event = await _eventService.GetEventByIdAsync(id);
        return Ok(@event);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventResponseDto>> CreateEvent([FromBody] EventCreateDto eventDto)
    {
        var createdEvent = await _eventService.CreateEventAsync(eventDto);
        return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventResponseDto>> UpdateEvent(Guid id, [FromBody] EventUpdateDto eventDto)
    {
        var updatedEvent = await _eventService.UpdateEventAsync(id, eventDto);
        return Ok(updatedEvent);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteEvent(Guid id)
    {
        await _eventService.DeleteEventAsync(id);
        return NoContent();
    }
}