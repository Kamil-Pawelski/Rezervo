using Application.Abstractions.Messaging;

namespace Application.Specialists.Get;

public sealed record GetSpecialistsQuery() : IQuery<List<SpecialistsResponse>>
{
}
