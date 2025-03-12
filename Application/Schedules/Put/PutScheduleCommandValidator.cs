using FluentValidation;

namespace Application.Schedules.Put;

internal sealed class PutScheduleCommandValidator : AbstractValidator<PutScheduleCommand>
{
    public PutScheduleCommandValidator()
    {
        RuleFor(command => command.StartTime)
            .NotEmpty();
        RuleFor(command => command.EndTime)
            .NotEmpty()
            .GreaterThan(command => command.StartTime);
    }
}
