using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Mapper;
using Domain.Common;
using Domain.Specialists;
using Microsoft.EntityFrameworkCore;

namespace Application.Specialists.GetById;

public sealed class GetByIdSpecialistQueryHandler(ISpecialistRepository specialistRepository) : IQueryHandler<GetByIdSpecialistQuery, SpecialistsResponse>
{
    public async Task<Result<SpecialistsResponse>> Handle(GetByIdSpecialistQuery query, CancellationToken cancellationToken)
    {
        Specialist? specialist = await specialistRepository.GetByIdAsync(query.Id, cancellationToken);

        if (specialist is null)
        {
            return Result.Failure<SpecialistsResponse>(SpecialistErrors.NotFoundSpecialist);
        }

        return Result.Success(specialist.MapToSpecialistResponse());
    }
}
