using Application.Specialists;
using Application.Specialists.Put;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specialists;

public sealed class Put : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut(
            "specialists/{id}", 
            [Authorize(Roles = RolesNames.Specialist)] async ([FromRoute] Guid id, [FromBody]PutSpecialistCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            Result<SpecialistsResponse> result = await sender.Send(command with { Id = id }, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
        });
}
