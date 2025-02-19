using Application.Abstractions.Messaging;

namespace Application.Users;

public sealed class RegisterUserCommand(string Email, string FirstName, string LastName, string Password)
    : ICommand<Guid>;

