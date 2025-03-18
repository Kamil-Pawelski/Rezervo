using Application.Users.Register;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Users;

public class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("users/register",
            async ([FromBody] RegisterUserCommand command, ISender sender, CancellationToken cancellationToken) =>
            {
                Result<string> result = await sender.Send(command, cancellationToken);
                
                return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
            });
}
