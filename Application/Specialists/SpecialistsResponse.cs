namespace Application.Specialists;

public class SpecialistsResponse
{
    public required Guid Id { get; set; }
    public required UserDto User { get; set; }
    public required SpecializationDto Specialization { get; set; }
    public required string Description { get; set; }
    public required string PhoneNumber { get; set; }

}

public sealed record UserDto(Guid Id, string FirstName, string LastName);
public sealed record SpecializationDto(Guid Id, string Name);
