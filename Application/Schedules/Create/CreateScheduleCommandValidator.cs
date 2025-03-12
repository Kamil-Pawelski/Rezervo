using FluentValidation;

namespace Application.Schedules.Create;

internal sealed class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
{
    public CreateScheduleCommandValidator()
    {
        RuleFor(command => command.SpecialistId)
            .NotEmpty();
        RuleFor(command => command.StartTime)
            .NotEmpty();
        RuleFor(command => command.EndTime)
            .NotEmpty()
            .GreaterThan(command => command.StartTime);
        RuleFor(command => command.Day)
            .NotEmpty();
        RuleFor(command => command.SlotDuration)
            .NotEmpty();
    }
}
