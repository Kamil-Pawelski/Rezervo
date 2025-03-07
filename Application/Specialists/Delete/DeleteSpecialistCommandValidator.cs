using FluentValidation;

namespace Application.Specialists.Delete;

internal sealed class DeleteSpecialistCommandValidator : AbstractValidator<DeleteSpecialistCommand>
{
    public DeleteSpecialistCommandValidator() =>
        RuleFor(command => command.Id)
            .NotEmpty();
}
