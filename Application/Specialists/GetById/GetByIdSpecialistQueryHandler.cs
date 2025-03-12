using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Specialists;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.GetById;

public sealed class GetByIdSpecialistQueryHandler(IApplicationDbContext context) : IQueryHandler<GetByIdSpecialistQuery, SpecialistsResponse>
{
    public async Task<Result<SpecialistsResponse>> Handle(GetByIdSpecialistQuery query, CancellationToken cancellationToken)
    {
        SpecialistsResponse? specialistResponse = await context.Specialists
            .Where(s => s.Id == query.Id)
            .Select(s => new SpecialistsResponse
            {
                Id = s.Id,
                User = new UserDto(s.User!.Id, s.User.FirstName, s.User.LastName),
                Specialization = new SpecializationDto(s.Specialization!.Id, s.Specialization.Name),
                PhoneNumber = s.PhoneNumber,
                Description = s.Description,
                City = s.City
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (specialistResponse is null)
        {
            return Result.Failure<SpecialistsResponse>(SpecialistErrors.NotFoundSpecialist);
        }

        return new Result<SpecialistsResponse>(specialistResponse, Error.None);
    }
}
