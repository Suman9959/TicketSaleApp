using System.ComponentModel.DataAnnotations;
using TicketSaleAPI.Models;

namespace TicketSaleAPI.DTOs;

public class EventCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    
    [Required]
    public EventCategory Category { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int TotalTickets { get; set; }
}

public class EventUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? Date { get; set; }
    public string? Location { get; set; }
    public decimal? Price { get; set; }
    public EventCategory? Category { get; set; }
    public int? TotalTickets { get; set; }
}

public class EventResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public int TotalTickets { get; set; }
    public int AvailableTickets { get; set; }
}