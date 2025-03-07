using FluentValidation;

namespace Application.Slots.Put;

internal sealed class PutSlotCommandValidator : AbstractValidator<PutSlotCommand>
{
    public PutSlotCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
        RuleFor(command => command.StartTime)
            .NotEmpty();
    }
}
