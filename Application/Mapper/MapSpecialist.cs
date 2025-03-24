using Application.Specialists;
using Domain.Specialists;

namespace Application.Mapper;

public static class MapSpecialist
{
    public static SpecialistsResponse MapToSpecialistResponse(this Specialist specialist) => new SpecialistsResponse(
    specialist.Id,
    specialist.User!.MapUserResponse(),
    specialist.Specialization!.MapSpecializationResponse(),
    specialist.Description,
    specialist.PhoneNumber,
    specialist.City
);
    public static List<SpecialistsResponse> MapToSpecialistResponseList(this List<Specialist> specialists) => [.. specialists.Select(specialist => specialist.MapToSpecialistResponse())];
}
