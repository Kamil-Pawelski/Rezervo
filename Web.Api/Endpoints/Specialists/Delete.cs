using Application.Specialists.Delete;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specialists;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
    app.MapDelete("specialists/{id}", [Authorize(Roles = RolesNames.Admin)] async ([FromRoute]Guid id , ISender sender, CancellationToken cancellationToken) =>
    {
        Result<string> result = await sender.Send(new DeleteSpecialistCommand(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(new { Message = result.Value }) : result.Error.MapError();
    });
}
