using Application.Abstractions.Messaging;
using Domain.Specializations;

namespace Application.Specializations.Get;

public sealed record GetSpecializationsQuery() : IQuery<List<SpecializationResponse>>;
