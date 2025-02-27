using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.Get;

public sealed class GetSpecialistsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSpecialistsQuery, List<SpecialistsResponse>>
{
    public async Task<Result<List<SpecialistsResponse>>> Handle(GetSpecialistsQuery query,
        CancellationToken cancellationToken)
    {

        List<SpecialistsResponse> result = await context.Specialists.AsNoTracking().Select(specialist => new SpecialistsResponse
        {
            Id = specialist.Id,
            User = new UserDto(specialist.User!.Id, specialist.User.FirstName, specialist.User.LastName),
            Specialization = new SpecializationDto(specialist.Specialization!.Id, specialist.Specialization.Name),
            PhoneNumber = specialist.PhoneNumber,
            Description = specialist.Description
        }).ToListAsync(cancellationToken);

        var specialistList = context.Specialists.ToList();

        return new Result<List<SpecialistsResponse>>(result, Error.None);
    }
}
