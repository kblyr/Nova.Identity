using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Controllers
{
    [ApiController, Route(ControllerRoutes.ClientApp)]
    public sealed class ClientAppController : ControllerBase
    {
        readonly IMediator _mediator;

        public ClientAppController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(ActionRoutes.ClientApp.Add)]
        public async Task<ActionResult<AddClientAppResponse>> Add([FromBody] AddClientAppRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);

        [HttpPost(ActionRoutes.ClientApp.Edit)]
        public async Task<ActionResult<EditClientAppResponse>> Edit([FromBody] EditClientAppRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);
    }
}