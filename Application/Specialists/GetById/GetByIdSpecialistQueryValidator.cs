using FluentValidation;

namespace Application.Specialists.GetById;

internal sealed class GetByIdSpecialistQueryValidator : AbstractValidator<GetByIdSpecialistQuery>
{
    public GetByIdSpecialistQueryValidator() =>
        RuleFor(query => query.Id)
            .NotEmpty();
}
