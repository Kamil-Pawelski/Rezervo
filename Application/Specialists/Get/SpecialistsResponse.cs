using Domain.Users;

namespace Application.Specialists.Get;

public class SpecialistsResponse
{
    public Guid Id { get; set; }
    public UserDto User { get; set; }
    public SpecializationDto Specialization { get; set; }
    public string Description { get; set; }
    public string PhoneNumber { get; set; }

}

public sealed record UserDto(Guid Id, string FirstName, string LastName);
public sealed record SpecializationDto(Guid Id, string Name);
