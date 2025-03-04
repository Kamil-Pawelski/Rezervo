using Application.Schedules.Put;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Schedule;

public class Put : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapPut("schedules/{id}", [Authorize(Roles = RolesNames.Specialist)] async ([FromRoute] Guid id, [FromBody] PutScheduleCommand command, ISender sender, CancellationToken cancellationToken) =>
    {
        Result<string> result = await sender.Send(command with { Id = id }, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
