using FluentValidation;

namespace Application.Bookings.Create;

internal sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator() =>
        RuleFor(c => c.SlotId)
            .NotEmpty();
}
