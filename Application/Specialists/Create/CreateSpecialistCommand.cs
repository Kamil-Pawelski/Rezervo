using Application.Abstractions.Messaging;

namespace Application.Specialists.Create;

public sealed record CreateSpecialistCommand(Guid UserId, string SpecializationName, string PhoneNumber, string Description, string City)
    : ICommand;
