using FluentValidation;

namespace Application.Users.Login;

internal sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(command => command.Login)
            .NotEmpty();
        RuleFor(command => command.Password)
            .NotEmpty();
    }
}
