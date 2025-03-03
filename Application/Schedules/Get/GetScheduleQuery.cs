using Application.Abstractions.Messaging;

namespace Application.Schedules.Get;

public sealed record GetScheduleQuery(Guid SpecialistId) : IQuery<List<ScheduleDateResponse>>;



