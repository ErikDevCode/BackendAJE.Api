namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Masters.Queries.GetAllReasonRequest;
    using BackEndAje.Api.Application.Masters.Queries.GetWithDrawalReason;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class MastersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MastersController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllReasonRequest()
        {
            var query = new GetAllReasonRequestQuery();
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetWithDrawalReason/{reasonRequestId}")]
        public async Task<IActionResult> GetWithDrawalReasonByReasonRequestId(int reasonRequestId)
        {
            var query = new GetWithDrawalReasonQuery(reasonRequestId);
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
        }
    }
}
