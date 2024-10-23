namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Masters.Queries.GetAllDocumentType;
    using BackEndAje.Api.Application.Masters.Queries.GetAllLogos;
    using BackEndAje.Api.Application.Masters.Queries.GetAllOrderStatus;
    using BackEndAje.Api.Application.Masters.Queries.GetAllPaymentMethod;
    using BackEndAje.Api.Application.Masters.Queries.GetAllProductSize;
    using BackEndAje.Api.Application.Masters.Queries.GetAllProductTypes;
    using BackEndAje.Api.Application.Masters.Queries.GetAllReasonRequest;
    using BackEndAje.Api.Application.Masters.Queries.GetAllTimeWindows;
    using BackEndAje.Api.Application.Masters.Queries.GetWithDrawalReason;
    using MediatR;
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
        [ProducesResponseType(typeof(GetAllReasonRequestResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllReasonRequest")]
        public async Task<IActionResult> GetAllReasonRequest()
        {
            var query = new GetAllReasonRequestQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetWithDrawalReasonResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetWithDrawalReason/{reasonRequestId}")]
        public async Task<IActionResult> GetWithDrawalReasonByReasonRequestId(int reasonRequestId)
        {
            var query = new GetWithDrawalReasonQuery(reasonRequestId);
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllTimeWindowsResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllTimeWindows")]
        public async Task<IActionResult> GetAllTimeWindows()
        {
            var query = new GetAllTimeWindowsQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllProductTypesResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllProductTypes")]
        public async Task<IActionResult> GetAllProductTypes()
        {
            var query = new GetAllProductTypesQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllLogosResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllLogos")]
        public async Task<IActionResult> GetAllLogos()
        {
            var query = new GetAllLogosQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllProductSizeResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllProductSize")]
        public async Task<IActionResult> GetAllProductSize()
        {
            var query = new GetAllProductSizeQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllPaymentMethodResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllPaymentMethod")]
        public async Task<IActionResult> GetAllPaymentMethod()
        {
            var query = new GetAllPaymentMethodQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllDocumentTypeResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllDocumentType")]
        public async Task<IActionResult> GetAllDocumentType()
        {
            var query = new GetAllDocumentTypeQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllOrderStatusResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetAllOrderRequestStatus")]
        public async Task<IActionResult> GetAllOrderRequestStatus()
        {
            var query = new GetAllOrderStatusQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }
    }
}
