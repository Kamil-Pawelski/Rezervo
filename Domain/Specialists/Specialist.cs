namespace Domain.Specialists;
public sealed class Specialist
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SpecializationId { get; set; }
    public string Description { get; set; }
    public string PhoneNumber { get; set; }
}
