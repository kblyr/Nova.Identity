using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Schema;

namespace Nova.Identity.Controllers
{
    public sealed class RoleController : BaseApiController
    {
        readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ActionRoutes.Role.Add)]
        public async Task<ActionResult<AddRoleOutput>> Add([FromBody] AddRoleInput input, CancellationToken cancellationToken) => AddRoleOutput.From(await _mediator.Send(input.ToRequest(), cancellationToken));

        [HttpPut(ActionRoutes.Role.Edit)]
        public async Task<ActionResult<EditRoleOutput>> Edit(int id, [FromBody] EditRoleInput input, CancellationToken cancellationToken) => EditRoleOutput.From(await _mediator.Send(input.ToRequest(id), cancellationToken));
    }
}