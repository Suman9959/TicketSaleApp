using System;
using TicketSaleApp.Models;
using TicketSaleApp.Utilities;

namespace TicketSaleApp.Services
{
    public class TicketService
    {
        public void PurchaseTicket(int eventId, int userId)
        {
            var ev = Database.Events.Find(e => e.EventID == eventId);
            if (ev == null || ev.AvailableTickets <= 0)
            {
                Console.WriteLine("Event not found or tickets sold out.");
                return;
            }

            ev.AvailableTickets--;

            var ticket = new Ticket
            {
                TicketID = Database.Tickets.Count + 1,
                EventID = eventId,
                UserID = userId,
                SeatNumber = ev.AvailableTickets + 1 // Simple seat allocation
            };

            Database.Tickets.Add(ticket);
            Console.WriteLine("Ticket purchased successfully!");
            Console.WriteLine($"Ticket ID: {ticket.TicketID}, Event: {ev.Name}, Seat: {ticket.SeatNumber}");
        }
    }
}
 