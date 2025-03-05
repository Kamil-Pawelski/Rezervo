using Application.Specializations;
using Application.Specializations.Get;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specializations;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
    app.MapGet("specializations/", [Authorize(Roles = RolesNames.Admin)] async (ISender sender, CancellationToken cancellationToken) =>
    {
        Result<List<SpecializationResponse>> result = await sender.Send(new GetSpecializationsQuery(), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
