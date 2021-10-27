using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Schema;

namespace Nova.Identity.Controllers
{
    public sealed class PermissionController : BaseApiController
    {
        readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ActionRoutes.Permission.Add)]
        public async Task<ActionResult<AddPermissionOutput>> Add([FromBody] AddPermissionInput input, CancellationToken cancellationToken) => AddPermissionOutput.From(await _mediator.Send(input.ToRequest(), cancellationToken));

        [HttpPut(ActionRoutes.Permission.Edit)]
        public async Task<ActionResult<EditPermissionOutput>> Edit(int id, [FromBody] EditPermissionInput input, CancellationToken cancellationToken) => EditPermissionOutput.From(await _mediator.Send(input.ToRequest(id), cancellationToken));
    }
}