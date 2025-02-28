using Application.Specialists.Create;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specialists;

public class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
    app.MapPost("specialists", 
        [Authorize(Roles = RolesNames.Specialist)]
        async ([FromBody]CreateSpecialistCommand createSpecialistCommand, ISender iSender, CancellationToken cancellationToken) =>
        {
            Result result = await iSender.Send(createSpecialistCommand, cancellationToken);

            return result.IsSuccess ? Results.Ok(new { Message = "The specialist account has been created." }) : result.Error.MapError();
        }
    );
}
