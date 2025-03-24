using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Common;
using Domain.Specialists;

namespace Application.Specialists.Get;

public sealed class GetSpecialistsQueryHandler(ISpecialistRepository specialistRepository) : IQueryHandler<GetSpecialistsQuery, List<SpecialistsResponse>>
{
    public async Task<Result<List<SpecialistsResponse>>> Handle(GetSpecialistsQuery query, CancellationToken cancellationToken)    
    {
        List<Specialist> result = await specialistRepository.GetAllAsync(cancellationToken);

        return Result.Success(result.MapToSpecialistResponseList());
    }
}
