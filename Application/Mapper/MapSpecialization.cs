using Application.Specializations;
using Domain.Specializations;

namespace Application.Mapper;

internal static class MapSpecialization
{
    public static SpecializationResponse MapSpecializationResponse(this Specialization specialization) => new SpecializationResponse(specialization.Id, specialization.Name);

    public static List<SpecializationResponse> MapToSpecializationResponseList(this List<Specialization> specializations) => [.. specializations.Select(s => s.MapSpecializationResponse())];
}
