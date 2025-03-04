using Application.Schedules.Delete;
using Domain.Common;
using Domain.Users;
using Infrastructure.Database.Migrations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Schedule;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapDelete("schedules/{id}", [Authorize(Roles = RolesNames.Specialist)]async ([FromRoute] Guid id, ISender sender, CancellationToken cancellationToken ) =>
    {
        Result<string> result = await sender.Send(new DeleteScheduleCommand(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
