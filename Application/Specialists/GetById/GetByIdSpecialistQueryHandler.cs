using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Mapper;
using Domain.Common;
using Domain.Specialists;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.GetById;

public sealed class GetByIdSpecialistQueryHandler(IApplicationDbContext context) : IQueryHandler<GetByIdSpecialistQuery, SpecialistsResponse>
{
    public async Task<Result<SpecialistsResponse>> Handle(GetByIdSpecialistQuery query, CancellationToken cancellationToken)
    {
        Specialist? specialist = await context.Specialists
            .Include(s => s.User)
            .Include(s => s.Specialization)
            .SingleOrDefaultAsync(s => s.Id == query.Id, cancellationToken);

        if (specialist is null)
        {
            return Result.Failure<SpecialistsResponse>(new Error("SpecialistNotFound", "Specialist with the given id does not exist", ErrorType.NotFound));
        }

        SpecialistsResponse specialistResponse = specialist.MapToSpecialistResponse();

        return new Result<SpecialistsResponse>(specialistResponse, Error.None);
    }
}
