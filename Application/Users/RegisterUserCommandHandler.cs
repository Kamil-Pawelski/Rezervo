using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;

namespace Application.Users;

public sealed class RegisterUserCommandHandler(IApplicationDbContext context)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        int x = 2;
        return null;
    }
}
