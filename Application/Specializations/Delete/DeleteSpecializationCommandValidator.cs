using FluentValidation;

namespace Application.Specializations.Delete;

internal sealed class DeleteSpecializationCommandValidator : AbstractValidator<DeleteSpecializationCommand>
{
    public DeleteSpecializationCommandValidator() =>
        RuleFor(c => c.Id)
            .NotEmpty();
}
