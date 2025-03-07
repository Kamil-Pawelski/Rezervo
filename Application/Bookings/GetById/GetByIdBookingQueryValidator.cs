using FluentValidation;

namespace Application.Bookings.GetById;

internal sealed class GetByIdBookingQueryValidator : AbstractValidator<GetByIdBookingQuery>
{
    public GetByIdBookingQueryValidator() =>
        RuleFor(query => query.Id)
            .NotEmpty();
}
