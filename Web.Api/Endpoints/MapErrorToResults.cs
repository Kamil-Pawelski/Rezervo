using Domain.Common;

namespace Web.Api.Endpoints;

public static class MapErrorToResults
{
    public static IResult MapError(Error error) =>
        error.Type switch
        {
            ErrorType.Validation => Results.BadRequest(new { error.Code, error.Description }),
            ErrorType.BadRequest => Results.BadRequest(new { error.Code, error.Description }),
            ErrorType.NotFound => Results.NotFound(new { error.Code, error.Description }),
            ErrorType.Conflict => Results.Conflict(new { error.Code, error.Description }),
            _ => Results.BadRequest(new { error.Code, error.Description })
        };
}
