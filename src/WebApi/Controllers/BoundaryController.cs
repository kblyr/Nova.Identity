using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Controllers
{
    [ApiController, Route(ControllerRoutes.Boundary)]
    public sealed class BoundaryController : ControllerBase
    {
        readonly IMediator _mediator;

        public BoundaryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ActionRoutes.Boundary.Add)]
        public async Task<ActionResult<AddBoundaryResponse>> Add([FromBody] AddBoundaryRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);

        [HttpPost(ActionRoutes.Boundary.Edit)]
        public async Task<ActionResult<EditBoundaryResponse>> Edit([FromBody] EditBoundaryRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);
    }
}