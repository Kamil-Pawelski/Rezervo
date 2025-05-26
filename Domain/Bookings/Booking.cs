using System.ComponentModel.DataAnnotations;
using Domain.Users;
using Domain.Slots;

namespace Domain.Bookings;
public sealed class Booking
{

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SlotId { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public Slot? Slot { get; set; }
}
