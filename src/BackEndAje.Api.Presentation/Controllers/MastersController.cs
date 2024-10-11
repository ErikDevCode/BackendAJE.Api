namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Masters.Queries.GetAllLogos;
    using BackEndAje.Api.Application.Masters.Queries.GetAllProductSize;
    using BackEndAje.Api.Application.Masters.Queries.GetAllProductTypes;
    using BackEndAje.Api.Application.Masters.Queries.GetAllReasonRequest;
    using BackEndAje.Api.Application.Masters.Queries.GetAllTimeWindows;
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
        [Route("GetAllReasonRequest")]
        public async Task<IActionResult> GetAllReasonRequest()
        {
            var query = new GetAllReasonRequestQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetWithDrawalReason/{reasonRequestId}")]
        public async Task<IActionResult> GetWithDrawalReasonByReasonRequestId(int reasonRequestId)
        {
            var query = new GetWithDrawalReasonQuery(reasonRequestId);
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllTimeWindows")]
        public async Task<IActionResult> GetAllTimeWindows()
        {
            var query = new GetAllTimeWindowsQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllProductTypes")]
        public async Task<IActionResult> GetAllProductTypes()
        {
            var query = new GetAllProductTypesQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllLogos")]
        public async Task<IActionResult> GetAllLogos()
        {
            var query = new GetAllLogosQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllProductSize")]
        public async Task<IActionResult> GetAllProductSize()
        {
            var query = new GetAllProductSizeQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }
    }
}
