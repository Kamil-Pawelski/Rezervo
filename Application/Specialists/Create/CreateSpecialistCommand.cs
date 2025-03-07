using Application.Abstractions.Messaging;

namespace Application.Specialists.Create;

public sealed record CreateSpecialistCommand(Guid UserId, Guid SpecializationId, string PhoneNumber, string Description, string City)
    : ICommand;
