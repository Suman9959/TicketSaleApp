using System.ComponentModel.DataAnnotations;

namespace TicketSaleAPI.Models;

public enum EventCategory
{
    Concert,
    Sports,
    Conference
}

public class Event
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public string Location { get; set; } = string.Empty;
    
    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public EventCategory Category { get; set; }
    
    [Required]
    public int TotalTickets { get; set; }
    
    [Required]
    public int AvailableTickets { get; set; }
    
    // Navigation property
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}