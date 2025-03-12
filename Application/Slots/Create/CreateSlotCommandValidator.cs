using Application.Abstractions.Data;
using Domain.Common;
using Domain.Schedules;
using FluentValidation;

namespace Application.Slots.Create;

internal sealed class CreateSlotCommandValidator : AbstractValidator<CreateSlotCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateSlotCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(command => command.ScheduleId)
            .NotEmpty();
        RuleFor(command => command.StartTime)
            .NotEmpty();
        RuleFor(command => command)
                .Must(ValidateScheduleTimeRange);
    }

    private bool ValidateScheduleTimeRange(CreateSlotCommand command)
    {
       Schedule? schedule = _context.Schedules.FirstOrDefault(s => s.Id == command.ScheduleId);

        if (schedule == null)
        {
            return true;
        }

        return command.StartTime >= schedule.StartTime && command.StartTime <= schedule.EndTime;
    }
}
