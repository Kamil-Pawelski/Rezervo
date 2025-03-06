using FluentValidation;

namespace Application.Bookings.Delete;

internal sealed class DeleteBookingCommandValidator : AbstractValidator<DeleteBookingCommand>
{
    public DeleteBookingCommandValidator() =>
        RuleFor(c => c.Id)
            .NotEmpty();
}
