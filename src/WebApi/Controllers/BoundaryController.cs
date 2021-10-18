using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Controllers
{
    [ApiController, Route("[controller]")]
    public sealed class BoundaryController : ControllerBase
    {
        readonly IMediator _mediator;

        public BoundaryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<ActionResult<AddBoundaryResponse>> Add([FromBody] AddBoundaryRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);
    }
}