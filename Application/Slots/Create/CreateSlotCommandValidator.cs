using FluentValidation;

namespace Application.Slots.Create;

internal sealed class CreateSlotCommandValidator : AbstractValidator<CreateSlotCommand>
{
    public CreateSlotCommandValidator()
    {
        RuleFor(command => command.ScheduleId)
            .NotEmpty();
        RuleFor(command => command.StartTime)
            .NotEmpty();
    }
}
