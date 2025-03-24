using FluentValidation;

namespace Application.Slots.Delete;

internal sealed class DeleteSlotCommandValidator : AbstractValidator<DeleteSlotCommand>
{
    public DeleteSlotCommandValidator() => 
        RuleFor(command => command.Id)
         .NotEmpty();
}
