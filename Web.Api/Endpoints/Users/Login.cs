using Application.Users.Login;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Users;

public class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("users/login",
            async ([FromBody] LoginUserCommand command, ISender ISender, CancellationToken cancellationToken) =>
            {
                Result<string> result = await ISender.Send(command, cancellationToken);

                return result.IsSuccess ? Results.Ok(new { Token = result.Value }) : result.Error.MapError();
            });
}
