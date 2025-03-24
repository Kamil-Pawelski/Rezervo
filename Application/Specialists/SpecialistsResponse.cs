using Application.Specializations;
using Application.Users;

namespace Application.Specialists;

public record SpecialistsResponse(
    Guid Id,
    UserResponse User,
    SpecializationResponse Specialization,
    string Description,
    string PhoneNumber,
    string City
);


