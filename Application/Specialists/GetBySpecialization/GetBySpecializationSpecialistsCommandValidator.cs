using FluentValidation;

namespace Application.Specialists.GetBySpecialization;

internal sealed class GetBySpecializationSpecialistsCommandValidator : AbstractValidator<GetBySpecializationSpecialistsCommand>
{
    public GetBySpecializationSpecialistsCommandValidator() =>
        RuleFor(command => command.Id)
            .NotEmpty();
}
