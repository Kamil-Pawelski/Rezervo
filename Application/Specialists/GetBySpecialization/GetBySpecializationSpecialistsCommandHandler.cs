using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Common;
using Domain.Specialists;

namespace Application.Specialists.GetBySpecialization;

public sealed class GetBySpecializationSpecialistsCommandHandler(ISpecialistRepository specialistRepository) : ICommandHandler<GetBySpecializationSpecialistsCommand, List<SpecialistsResponse>> 
{
    public async Task<Result<List<SpecialistsResponse>>> Handle(GetBySpecializationSpecialistsCommand command,
        CancellationToken cancellationToken)
    {
        List<Specialist> result = await specialistRepository.GetBySpecializationAsync(command.Id, cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<SpecialistsResponse>>(SpecialistErrors.NotFoundSpecialist);
        }

        return Result.Success(result.MapToSpecialistResponseList());
    }
}
