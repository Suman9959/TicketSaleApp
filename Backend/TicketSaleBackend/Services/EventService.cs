using Microsoft.EntityFrameworkCore;
using TicketSaleAPI.Data;
using TicketSaleAPI.DTOs;
using TicketSaleAPI.Models;

namespace TicketSaleAPI.Services;

public interface IEventService
{
    Task<IEnumerable<EventResponseDto>> GetAllEventsAsync();
    Task<EventResponseDto> GetEventByIdAsync(Guid id);
    Task<EventResponseDto> CreateEventAsync(EventCreateDto eventDto);
    Task<EventResponseDto> UpdateEventAsync(Guid id, EventUpdateDto eventDto);
    Task DeleteEventAsync(Guid id);
}

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;

    public EventService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EventResponseDto>> GetAllEventsAsync()
    {
        var events = await _context.Events.ToListAsync();
        return events.Select(MapToEventResponseDto);
    }

    public async Task<EventResponseDto> GetEventByIdAsync(Guid id)
    {
        var @event = await _context.Events.FindAsync(id) 
            ?? throw new KeyNotFoundException($"Event with ID {id} not found");
        
        return MapToEventResponseDto(@event);
    }

    public async Task<EventResponseDto> CreateEventAsync(EventCreateDto eventDto)
    {
        var @event = new Event
        {
            Id = Guid.NewGuid(),
            Title = eventDto.Title,
            Description = eventDto.Description,
            Date = eventDto.Date,
            Location = eventDto.Location,
            Price = eventDto.Price,
            Category = eventDto.Category,
            TotalTickets = eventDto.TotalTickets,
            AvailableTickets = eventDto.TotalTickets
        };

        await _context.Events.AddAsync(@event);
        await _context.SaveChangesAsync();

        return MapToEventResponseDto(@event);
    }

    public async Task<EventResponseDto> UpdateEventAsync(Guid id, EventUpdateDto eventDto)
    {
        var @event = await _context.Events.FindAsync(id)
            ?? throw new KeyNotFoundException($"Event with ID {id} not found");

        // Update only the provided properties
        if (eventDto.Title != null)
            @event.Title = eventDto.Title;
        
        if (eventDto.Description != null)
            @event.Description = eventDto.Description;
        
        if (eventDto.Date.HasValue)
            @event.Date = eventDto.Date.Value;
        
        if (eventDto.Location != null)
            @event.Location = eventDto.Location;
        
        if (eventDto.Price.HasValue)
            @event.Price = eventDto.Price.Value;
        
        if (eventDto.Category.HasValue)
            @event.Category = eventDto.Category.Value;
        
        if (eventDto.TotalTickets.HasValue)
        {
            int difference = eventDto.TotalTickets.Value - @event.TotalTickets;
            @event.TotalTickets = eventDto.TotalTickets.Value;
            @event.AvailableTickets += difference;
        }

        _context.Events.Update(@event);
        await _context.SaveChangesAsync();

        return MapToEventResponseDto(@event);
    }

    public async Task DeleteEventAsync(Guid id)
    {
        var @event = await _context.Events.FindAsync(id)
            ?? throw new KeyNotFoundException($"Event with ID {id} not found");

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();
    }

    private static EventResponseDto MapToEventResponseDto(Event @event)
    {
        return new EventResponseDto
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            Date = @event.Date,
            Location = @event.Location,
            Price = @event.Price,
            Category = @event.Category.ToString(),
            TotalTickets = @event.TotalTickets,
            AvailableTickets = @event.AvailableTickets
        };
    }
}