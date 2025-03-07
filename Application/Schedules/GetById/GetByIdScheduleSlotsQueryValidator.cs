using FluentValidation;

namespace Application.Schedules.GetById;

internal sealed class GetByIdScheduleSlotsQueryValidator : AbstractValidator<GetByIdScheduleSlotsQuery>
{
    public GetByIdScheduleSlotsQueryValidator() =>
        RuleFor(query => query.ScheduleId)
            .NotEmpty();
}
