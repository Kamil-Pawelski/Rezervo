using Application.Abstractions.Messaging;

namespace Application.Schedules.GetById;

public sealed record GetByIdScheduleSlotsQuery(Guid ScheduleId) : IQuery<List<SlotResponse>>;
