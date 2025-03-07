using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSaleAPI.Models;

public enum TicketStatus
{
    Confirmed,
    Canceled
}

public class Ticket
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid EventId { get; set; }
    
    [Required]
    public DateTime BookingDate { get; set; }
    
    [Required]
    public TicketStatus Status { get; set; } = TicketStatus.Confirmed;
    
    // Navigation properties
    [ForeignKey("UserId")]
    public User? User { get; set; }
    
    [ForeignKey("EventId")]
    public Event? Event { get; set; }
}