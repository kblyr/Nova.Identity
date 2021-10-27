using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nova.Identity.Schema;

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
        public async Task<ActionResult<AddClientAppOutput>> Add([FromBody] AddClientAppInput input, CancellationToken cancellationToken) => AddClientAppOutput.From(await _mediator.Send(input.ToRequest(), cancellationToken));

        [HttpPut(ActionRoutes.ClientApp.Edit)]
        public async Task<ActionResult<EditClientAppOutput>> Edit(short id, [FromBody] EditClientAppInput input, CancellationToken cancellationToken) => EditClientAppOutput.From(await _mediator.Send(input.ToRequest(id), cancellationToken));
    }
}