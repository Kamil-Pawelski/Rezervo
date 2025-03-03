namespace Application.Schedules;

public sealed class ScheduleDateResponse
{
    public Guid ScheduleId { get; set; }
    public DateOnly Date { get; set; }
}

