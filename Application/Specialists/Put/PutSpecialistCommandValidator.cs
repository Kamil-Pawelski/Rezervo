using FluentValidation;

namespace Application.Specialists.Put;

internal sealed class PutSpecialistCommandValidator : AbstractValidator<PutSpecialistCommand>
{
    public PutSpecialistCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
        RuleFor(command => command.City)
            .NotEmpty();
        RuleFor(command => command.Description)
            .NotEmpty();
        RuleFor(command => command.PhoneNumber)
            .NotEmpty();
        RuleFor(command => command.UserId)
            .NotEmpty();
    }
}
