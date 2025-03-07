using System.ComponentModel.DataAnnotations;

namespace TicketSaleAPI.Models;

public enum UserRole
{
    User,
    Admin
}

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string FullName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    public UserRole Role { get; set; } = UserRole.User;
    
    // Navigation property
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}