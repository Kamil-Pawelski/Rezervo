using Application.Users.Register;
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
                
                return result.IsSuccess ? Results.Ok(new {Message = "The account has been created."}) : MapErrorToResults.MapError(result.Error);
            });
}
