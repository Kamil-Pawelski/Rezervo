using FluentValidation;

namespace Application.Specializations.Create;

internal sealed class CreateSpecializationCommandValidator : AbstractValidator<CreateSpecializationCommand>
{
    public CreateSpecializationCommandValidator() =>
        RuleFor(command => command.Name)
            .NotEmpty();
}
