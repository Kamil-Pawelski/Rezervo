using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Mapper;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.GetBySpecialization;

public sealed class GetBySpecializationSpecialistsCommandHandler(IApplicationDbContext context) : ICommandHandler<GetBySpecializationSpecialitsCommand, List<SpecialistsResponse>> 
{
    public async Task<Result<List<SpecialistsResponse>>> Handle(GetBySpecializationSpecialitsCommand command,
        CancellationToken cancellationToken)
    {
        List<SpecialistsResponse> result = await context.Specialists
            .Include(s => s.User)
            .Include(s => s.Specialization)
            .Where(s => s.SpecializationId == command.Id)
            .Select(s => s.MapToSpecialistResponse())
            .ToListAsync(cancellationToken);
        
        if (result.Count == 0)
        {
            return Result.Failure<List<SpecialistsResponse>>(new Error("SpecialistsNotFound", "Specialist with the given specialization id does not exist", ErrorType.NotFound));
        }

        return new Result<List<SpecialistsResponse>>(result, Error.None);
    }
}
