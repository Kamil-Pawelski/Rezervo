using Application.Abstractions.Messaging;

namespace Application.Specialists.Put;

public sealed record PutSpecialistCommand(Guid Id, Guid UserId, string PhoneNumber, string Descriptions, string City) : ICommand<SpecialistsResponse>; 
