using Application.Schedules.Create;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Schedule;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("schedules", [Authorize(Roles = RolesNames.Specialist)] async (CreateScheduleCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            Result<string> result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
        });
}
