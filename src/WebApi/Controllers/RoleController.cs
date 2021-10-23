using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Controllers
{
    [ApiController, Route(ControllerRoutes.Role)]
    public sealed class RoleController : ControllerBase
    {
        readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ActionRoutes.Role.Add)]
        public async Task<ActionResult<AddRoleResponse>> Add([FromBody] AddRoleRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);

        [HttpPost(ActionRoutes.Role.Edit)]
        public async Task<ActionResult<EditRoleResponse>> Edit([FromBody] EditRoleRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);
    }
}