using Application.Abstractions.Messaging;

namespace Application.Schedules.GetById;

public sealed record GetScheduleSlotsByIdQuery(Guid ScheduleId) : IQuery<List<SlotResponse>>;
