using Application.Users;
using Domain.Common;
using MediatR;

namespace Web.Api.Endpoints.Users;

public class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("users/register",
            async (RegisterUserCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                Result result = await sender.Send(command, cancellationToken);
                
                return result.IsSuccess ? Results.Ok() : MapErrorToResults.MapError(result.Error);
            });
}
