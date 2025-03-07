using FluentValidation;

namespace Application.Schedules.Delete;

internal sealed class DeleteScheduleCommandValidator : AbstractValidator<DeleteScheduleCommand>
{
    public DeleteScheduleCommandValidator() =>
        RuleFor(c => c.Id)
            .NotEmpty();
}
