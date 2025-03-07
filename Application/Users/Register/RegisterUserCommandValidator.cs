using FluentValidation;

namespace Application.Users.Register;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(command => command.Password)
            .NotEmpty();
        RuleFor(command => command.FirstName)
            .NotEmpty();
        RuleFor(command => command.LastName)
            .NotEmpty();
        RuleFor(command => command.RoleId)
            .NotEmpty();
    }
}
