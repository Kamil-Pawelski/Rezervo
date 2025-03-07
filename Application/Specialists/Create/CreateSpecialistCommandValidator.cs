using FluentValidation;

namespace Application.Specialists.Create;

internal sealed class CreateSpecialistCommandValidator : AbstractValidator<CreateSpecialistCommand>
{
    public CreateSpecialistCommandValidator()
    {
        RuleFor(command => command.City)
            .NotEmpty();
        RuleFor(command => command.Description)
            .NotEmpty();
        RuleFor(command => command.PhoneNumber)
            .NotEmpty();
        RuleFor(command => command.SpecializationId)
            .NotEmpty();
        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}
