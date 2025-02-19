using Application.Abstractions.Data;

namespace Application.Users;

public sealed record RegisterUserCommandHandler(IApplicationDbContext context)
{
}
