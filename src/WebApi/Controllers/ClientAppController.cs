using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Requests;
using Nova.Identity.Responses;

namespace Nova.Identity.Controllers
{
    public sealed class ClientAppController : BaseApiController
    {
        readonly IMediator _mediator;

        public ClientAppController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(ActionRoutes.ClientApp.Add)]
        public async Task<ActionResult<AddClientAppResponse>> Add([FromBody] AddClientAppRequest request, CancellationToken cancellationToken) => await _mediator.Send(request, cancellationToken);

        [HttpPut(ActionRoutes.ClientApp.Edit)]
        public async Task<ActionResult<EditClientAppResponse>> Edit(short id, [FromBody] EditClientAppRequest request, CancellationToken cancellationToken) => await _mediator.Send(request with { Id = id }, cancellationToken);
    }
}