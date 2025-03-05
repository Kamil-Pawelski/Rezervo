using Application.Specializations.Create;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specializations;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapPost("/specializations", [Authorize(Roles = RolesNames.Admin)] async ([FromBody] CreateSpecializationCommand command, ISender sender, CancellationToken cancellationToken) =>
    {
        Result<string> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
