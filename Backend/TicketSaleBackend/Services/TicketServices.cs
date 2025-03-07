using Microsoft.EntityFrameworkCore;
using TicketSaleAPI.Data;
using TicketSaleAPI.DTOs;
using TicketSaleAPI.Models;

namespace TicketSaleAPI.Services;

public interface ITicketService
{
    Task<TicketResponseDto> BookTicketAsync(Guid userId, TicketCreateDto ticketDto);
    Task<IEnumerable<TicketResponseDto>> GetUserTicketsAsync(Guid userId);
    Task CancelTicketAsync(Guid ticketId, Guid userId);
}

public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;

    public TicketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TicketResponseDto> BookTicketAsync(Guid userId, TicketCreateDto ticketDto)
    {
        // Check if event exists and has available tickets
        var @event = await _context.Events.FindAsync(ticketDto.EventId)
            ?? throw new KeyNotFoundException($"Event with ID {ticketDto.EventId} not found");

        if (@event.AvailableTickets <= 0)
        {
            throw new InvalidOperationException("No tickets available for this event");
        }

        // Create ticket
        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EventId = ticketDto.EventId,
            BookingDate = DateTime.UtcNow,
            Status = TicketStatus.Confirmed
        };

        // Update available tickets
        @event.AvailableTickets--;

        await _context.Tickets.AddAsync(ticket);
        _context.Events.Update(@event);
        await _context.SaveChangesAsync();

        return new TicketResponseDto
        {
            Id = ticket.Id,
            EventId = ticket.EventId,
            EventTitle = @event.Title,
            EventDate = @event.Date,
            EventLocation = @event.Location,
            BookingDate = ticket.BookingDate,
            Status = ticket.Status.ToString()
        };
    }

    public async Task<IEnumerable<TicketResponseDto>> GetUserTicketsAsync(Guid userId)
    {
        var tickets = await _context.Tickets
            .Where(t => t.UserId == userId)
            .Include(t => t.Event)
            .ToListAsync();

        return tickets.Select(t => new TicketResponseDto
        {
            Id = t.Id,
            EventId = t.EventId,
            EventTitle = t.Event?.Title ?? "Unknown Event",
            EventDate = t.Event?.Date ?? DateTime.MinValue,
            EventLocation = t.Event?.Location ?? "Unknown Location",
            BookingDate = t.BookingDate,
            Status = t.Status.ToString()
        });
    }

    public async Task CancelTicketAsync(Guid ticketId, Guid userId)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Event)
            .FirstOrDefaultAsync(t => t.Id == ticketId && t.UserId == userId)
            ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found for this user");

        if (ticket.Status == TicketStatus.Canceled)
        {
            throw new InvalidOperationException("Ticket is already canceled");
        }

        // Update ticket status
        ticket.Status = TicketStatus.Canceled;

        // Increase available tickets for the event
        if (ticket.Event != null)
        {
            ticket.Event.AvailableTickets++;
        }

        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }
}