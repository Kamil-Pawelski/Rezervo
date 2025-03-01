using Application.Specialists;
using Application.Specialists.GetBySpecialization;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specialists;

public class GetBySpecialization : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapGet("specialists/specialization/{id}", async ([FromRoute] Guid id, ISender iSender, CancellationToken cancellationToken) =>
    {
        Result<List<SpecialistsResponse>> result = await iSender.Send(new GetBySpecializationSpecialitsCommand(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
