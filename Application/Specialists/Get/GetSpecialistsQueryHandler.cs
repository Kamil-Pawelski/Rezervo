using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.Get;

public sealed class GetSpecialistsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSpecialistsQuery, List<SpecialistsResponse>>
{
    public async Task<Result<List<SpecialistsResponse>>> Handle(GetSpecialistsQuery query,
        CancellationToken cancellationToken)    {

        List<SpecialistsResponse> result = await context.Specialists
        .Select(s => new SpecialistsResponse
        {
            Id = s.Id,
            User = new UserDto(s.User!.Id, s.User.FirstName, s.User.LastName),
            Specialization = new SpecializationDto(s.Specialization!.Id, s.Specialization.Name),
            PhoneNumber = s.PhoneNumber,
            Description = s.Description,
            City = s.City
        })
        .ToListAsync(cancellationToken);

        return new Result<List<SpecialistsResponse>>(result, Error.None);
    }
}
