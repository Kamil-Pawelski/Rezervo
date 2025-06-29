using System.Reflection;
using Domain.Common;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.Abstractions.Behavior;

internal sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationFailure[] validationFailures = await ValidateAsync(request);

        if (validationFailures.Length == 0)
        {
            return await next();
        }

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type resultType = typeof(TResponse).GetGenericArguments()[0];

            MethodInfo? failureMethod = typeof(Result<>)
                .MakeGenericType(resultType)
                .GetMethod(nameof(Result<object>.Failure)) ?? throw new ValidationException(validationFailures);

            var error = Error.Validation(
                "ValidationError",
                "Validation failed"
            );

            return (TResponse)failureMethod.Invoke(
                null,
                [error])!;
        }
        else if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(
                Error.Validation(
                    "ValidationError",
                    "Validation failed"));
        }

        throw new ValidationException(validationFailures);
    }


    private async Task<ValidationFailure[]> ValidateAsync(TRequest request)
    {
        if (!validators.Any())
        {
            return [];
        }

        var context = new ValidationContext<TRequest>(request);

        ValidationResult[] validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context)));

        ValidationFailure[] validationFailures = validationResults.Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .ToArray();

        return validationFailures;
    } 
}
