using Application.Abstractions.Messaging;

namespace Application.Users;

public sealed record RegisterUserCommand(string Email, string Username, string FirstName, string LastName, string Password)
    : ICommand;

