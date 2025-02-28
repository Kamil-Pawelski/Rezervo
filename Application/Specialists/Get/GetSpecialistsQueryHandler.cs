using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Mapper;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.Get;

public sealed class GetSpecialistsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSpecialistsQuery, List<SpecialistsResponse>>
{
    public async Task<Result<List<SpecialistsResponse>>> Handle(GetSpecialistsQuery query,
        CancellationToken cancellationToken)
    {

        List<SpecialistsResponse> result = await context.Specialists
            .Include(s => s.User)
            .Include(s => s.Specialization)
            .Select(specialist => specialist.MapToSpecialistResponse())
            .ToListAsync(cancellationToken);

        return new Result<List<SpecialistsResponse>>(result, Error.None);
    }
}
