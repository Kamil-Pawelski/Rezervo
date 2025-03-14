﻿using Application.Slots.Create;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Slots;

public class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapPost("slots/", [Authorize(Roles = RolesNames.Specialist)] async ([FromBody]CreateSlotCommand command, ISender sender, CancellationToken cancellationToken) =>
    {
        Result<string> result = await sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
