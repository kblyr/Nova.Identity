using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Schema;

namespace Nova.Identity.Controllers
{
    public sealed class BoundaryController : BaseApiController
    {
        readonly IMediator _mediator;

        public BoundaryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ActionRoutes.Boundary.Add)]
        public async Task<ActionResult<AddBoundaryOutput>> Add([FromBody] AddBoundaryInput input, CancellationToken cancellationToken) => AddBoundaryOutput.From(await _mediator.Send(input.ToRequest(), cancellationToken));

        [HttpPut(ActionRoutes.Boundary.Edit)]
        public async Task<ActionResult<EditBoundaryOutput>> Edit(short id, [FromBody] EditBoundaryInput input, CancellationToken cancellationToken) => EditBoundaryOutput.From(await _mediator.Send(input.ToRequest(id), cancellationToken));
    }
}