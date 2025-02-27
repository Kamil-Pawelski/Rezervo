using Application.Abstractions.Messaging;

namespace Application.Specialists.GetById;

public sealed record GetByIdSpecialistQuery(Guid Id) : IQuery<SpecialistsResponse>;

