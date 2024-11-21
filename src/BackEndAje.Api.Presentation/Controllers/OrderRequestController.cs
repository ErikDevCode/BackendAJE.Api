namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.OrderRequestAssets.Commands.AssignAssetsToOrderRequest;
    using BackEndAje.Api.Application.OrderRequestAssets.Commands.DeleteAssetToOrderRequest;
    using BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById;
    using BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Documents.Commands.CreateDocumentByOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Documents.Commands.DeleteDocumentByOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetOrderRequestById;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingAssetsByOrderRequestId;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingByOrderRequestId;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class OrderRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderRequestController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("create")]
        public async Task<IActionResult> CreateOrderRequest([FromBody] CreateOrderRequestsCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllOrderRequestsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAllOrderRequests(
            [FromQuery] int? pageNumber = 1,
            [FromQuery] int? pageSize = 10,
            [FromQuery] int? ClientCode = null,
            [FromQuery] int? StatusCode = null,
            [FromQuery] int? ReasonRequestId = null,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null)
        {
            var query = new GetAllOrderRequestsQuery(pageNumber ?? 1, pageSize ?? 10, ClientCode, StatusCode, ReasonRequestId, StartDate, EndDate);
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
        }

        [HttpGet("{orderRequestId}")]
        [ProducesResponseType(typeof(GetOrderRequestByIdResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderRequestById(int orderRequestId)
        {
            var result = await this._mediator.Send(new GetOrderRequestByIdQuery(orderRequestId));

            if (result == null)
            {
                return this.NotFound(new { Message = $"Solicitud con ID {orderRequestId} no encontrado." });
            }

            return this.Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("updateStatusOrderRequest")]
        public async Task<IActionResult> UpdateStatusOrderRequest([FromBody] UpdateStatusOrderRequestCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetTrackingByOrderRequestIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.NotFound)]
        [Route("tracking/{orderRequestId}")]
        public async Task<IActionResult> GetTrackingByOrderRequestId(int orderRequestId)
        {
            var result = await this._mediator.Send(new GetTrackingByOrderRequestIdQuery(orderRequestId));

            if (result == null)
            {
                return this.NotFound(new { Message = $"Solicitud con ID {orderRequestId} no encontrado." });
            }

            return this.Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("documents/create")]
        public async Task<IActionResult> CreateDocumentByOrderRequest([FromForm] CreateDocumentByOrderRequestCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("documents/delete")]
        public async Task<IActionResult> DeleteDocumentByOrderRequest([FromBody] DeleteDocumentByOrderRequestCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet("documents/by-orderrequestid/{orderRequestId}")]
        public async Task<IActionResult> DocumentsByOrderRequestId(int orderRequestId)
        {
            var documents = await this._mediator.Send(new GetOrderRequestDocumentByIdQuery(orderRequestId));

            if (documents == null)
            {
                return this.NotFound(new { Message = $"No se encontraron documentos para el OrderRequest con ID: {orderRequestId}." });
            }

            return this.Ok(new Response { Result = documents });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("assets/assign-assets")]
        public async Task<IActionResult> AssignAssetsToOrderRequest([FromBody] AssignAssetsToOrderRequestCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("assets/delete-asset")]
        public async Task<IActionResult> DeleteAssetToOrderRequest([FromBody] DeleteAssetToOrderRequestCommand command)
        {
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetTrackingAssetsByOrderRequestIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.NotFound)]
        [Route("tracking/assets/{orderRequestId}")]
        public async Task<IActionResult> GetTrackingAssetsByOrderRequestId(int orderRequestId)
        {
            var result = await this._mediator.Send(new GetTrackingAssetsByOrderRequestIdQuery(orderRequestId));

            if (result == null)
            {
                return this.NotFound(new { Message = $"Solicitud con ID {orderRequestId} no encontrado." });
            }

            return this.Ok(result);
        }
    }
}
