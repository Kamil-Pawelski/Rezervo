using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Specializations.Get;

public sealed class GetSpecializationsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSpecializationsQuery, List<SpecializationResponse>>
{
    public async Task<Result<List<SpecializationResponse>>> Handle(GetSpecializationsQuery request,
        CancellationToken cancellationToken)
    {
        List<SpecializationResponse> result = await context.Specializations
            .Select(specialization => new SpecializationResponse
            (
                 specialization.Id,
                 specialization.Name
            ))
            .ToListAsync(cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<SpecializationResponse>>(new Error("NotFoundSpecializations", "No specializations found", ErrorType.NotFound));
        }

        return Result.Success(result);
    }
}
