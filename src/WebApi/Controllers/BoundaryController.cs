using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

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
        public async Task<ActionResult<AddBoundaryResponse>> Add([FromBody] AddBoundaryRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);

        [HttpPut(ActionRoutes.Boundary.Edit)]
        public async Task<ActionResult<EditBoundaryResponse>> Edit(short id, [FromBody] EditBoundaryRequest request, CancellationToken cancellationToken) => await _mediator.Send(request with { Id = id }, cancellationToken);
    }
}