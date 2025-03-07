using FluentValidation;

namespace Application.Schedules.Put;

internal sealed class PutScheduleCommandValidator : AbstractValidator<PutScheduleCommand>
{
    public PutScheduleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
        RuleFor(command => command.EndTime)
            .NotEmpty();
        RuleFor(command => command.StartTime)
            .NotEmpty();
    }
}
