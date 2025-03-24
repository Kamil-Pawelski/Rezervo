using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Common;
using Domain.Specializations;

namespace Application.Specializations.Get;

public sealed class GetSpecializationsQueryHandler(ISpecializationRepository specializationRepository) : IQueryHandler<GetSpecializationsQuery, List<SpecializationResponse>>
{
    public async Task<Result<List<SpecializationResponse>>> Handle(GetSpecializationsQuery request,
        CancellationToken cancellationToken)
    {
        List<Specialization> result = await specializationRepository.GetAllAsync(cancellationToken);

        if (result.Count == 0)
        {
            return Result.Failure<List<SpecializationResponse>>(SpecializationErrors.NotFoundSpecializations);
        }

        return Result.Success(result.MapToSpecializationResponseList());
    }
}
