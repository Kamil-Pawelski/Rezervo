using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Specialists;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.GetBySpecialization;

public sealed class GetBySpecializationSpecialistsCommandHandler(IApplicationDbContext context) : ICommandHandler<GetBySpecializationSpecialistsCommand, List<SpecialistsResponse>> 
{
    public async Task<Result<List<SpecialistsResponse>>> Handle(GetBySpecializationSpecialistsCommand command,
        CancellationToken cancellationToken)
    {
        List<SpecialistsResponse> result = await context.Specialists
            .Where(s => s.SpecializationId == command.Id)
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

        if (result.Count == 0)
        {
            return Result.Failure<List<SpecialistsResponse>>(SpecialistErrors.NotFoundSpecialist);
        }

        return new Result<List<SpecialistsResponse>>(result, Error.None);
    }
}
