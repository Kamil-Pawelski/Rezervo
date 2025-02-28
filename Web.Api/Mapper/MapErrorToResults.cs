using Domain.Common;

namespace Web.Api.Mapper;

public static class MapErrorToResults
{
    public static IResult MapError(this Error error) =>
        error.Type switch
        {
            ErrorType.Validation => Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { error.Code, [error.Description] }
            }),
            ErrorType.BadRequest => Results.BadRequest(new { error.Code, error.Description }),
            ErrorType.NotFound => Results.NotFound(new { error.Code, error.Description }),
            ErrorType.Conflict => Results.Conflict(new { error.Code, error.Description }),
            ErrorType.Unauthorized => Results.Json(new { error.Code, error.Description }, statusCode: StatusCodes.Status401Unauthorized),
            ErrorType.Forbidden => Results.Json(new { error.Code, error.Description }, statusCode: StatusCodes.Status403Forbidden),
            _ => Results.BadRequest(new { error.Code, error.Description })
        };
}
