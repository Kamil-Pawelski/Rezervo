using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Common;
using Domain.Schedules;
using Microsoft.EntityFrameworkCore;

namespace Application.Schedules.Get;

public sealed class GetScheduleQueryHandler(IScheduleRepository scheduleRepository) : IQueryHandler<GetScheduleQuery, List<ScheduleResponse>>
{
    public async Task<Result<List<ScheduleResponse>>> Handle(GetScheduleQuery query, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        List<Schedule> result = await scheduleRepository.GetBySpecialistAsync(query.SpecialistId, today, cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<ScheduleResponse>>(ScheduleErrors.NoAvailableSlots);
        }

        return Result.Success(result.MapToScheduleResponse());
    }
}
