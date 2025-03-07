using System.ComponentModel.DataAnnotations;
using TicketSaleAPI.Models;

namespace TicketSaleAPI.DTOs;

public class TicketCreateDto
{
    [Required]
    public Guid EventId { get; set; }
}

public class TicketResponseDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string EventLocation { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public string Status { get; set; } = string.Empty;
}