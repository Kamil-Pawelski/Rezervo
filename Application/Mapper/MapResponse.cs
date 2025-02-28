using Application.Specialists;
using Domain.Specialists;

namespace Application.Mapper;

public static class MapResponse
{
    public static SpecialistsResponse MapToSpecialistResponse(this Specialist specialist) =>
        new()
        {
            Id = specialist.Id,
            User = new UserDto(specialist.User!.Id, specialist.User.FirstName, specialist.User.LastName),
            Specialization = new SpecializationDto(specialist.Specialization!.Id, specialist.Specialization.Name),
            PhoneNumber = specialist.PhoneNumber,
            Description = specialist.Description,
            City = specialist.City
        };
}
