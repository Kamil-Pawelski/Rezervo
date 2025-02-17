namespace Domain.Bookings;
public class Booking
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ScheduleId { get; set; }
    public DateTime Created { get; set; }
}
