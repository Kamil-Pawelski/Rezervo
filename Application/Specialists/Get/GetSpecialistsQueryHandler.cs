using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Specialists;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.Get;

public sealed class GetSpecialistsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSpecialistsQuery, List<SpecialistsResponse>>
{
    public async Task<Result<List<SpecialistsResponse>>> Handle(GetSpecialistsQuery request,
        CancellationToken cancellationToken)
    {

        List<SpecialistsResponse> result = await context.Specialists.AsNoTracking().Select(specialist => new SpecialistsResponse
        {
            Id = specialist.Id,
            User = new UserDto(specialist.User.Id, specialist.User.FirstName, specialist.User.LastName),
            Specialization = new SpecializationDto(specialist.Specialization.Id, specialist.Specialization.Name),
            PhoneNumber = specialist.PhoneNumber,
            Description = specialist.Description
        }).ToListAsync(cancellationToken);

        return new Result<List<SpecialistsResponse>>(result, Error.None);
    }
}
