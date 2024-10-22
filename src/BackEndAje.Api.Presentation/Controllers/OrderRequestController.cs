namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById;
    using BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusDocument;
    using BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetOrderRequestById;
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
            var userId = this.GetUserId();
            command.CreatedBy = userId;
            command.UpdatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAllOrderRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllOrderRequestsQuery(pageNumber, pageSize);
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
        }

        [HttpGet("{orderRequestId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderRequestById(int orderRequestId)
        {
            var result = await this._mediator.Send(new GetOrderRequestByIdQuery(orderRequestId));

            if (result == null)
            {
                return this.NotFound(new { Message = $"Order request with ID {orderRequestId} not found." });
            }

            return this.Ok(result);
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("updateStatusOrderRequest")]
        public async Task<IActionResult> UpdateStatusOrderRequest([FromBody] UpdateStatusOrderRequestCommand command)
        {
            var userId = this.GetUserId();
            command.CreatedBy = userId;
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.NotFound)]
        [Route("tracking/{orderRequestId}")]
        public async Task<IActionResult> GetTrackingByOrderRequestId(int orderRequestId)
        {
            var result = await this._mediator.Send(new GetTrackingByOrderRequestIdQuery(orderRequestId));

            if (result == null)
            {
                return this.NotFound(new { Message = $"Order request with ID {orderRequestId} not found." });
            }

            return this.Ok(result);
        }

        [HttpGet("documents/{documentId}/download")]
        public async Task<IActionResult> DownloadDocument(int documentId)
        {
            var result = await this._mediator.Send(new GetOrderRequestDocumentByIdQuery(documentId));

            if (result == null)
            {
                return this.NotFound(new { Message = $"Order request with ID {documentId} not found." });
            }

            return this.File(result.DocumentContent, result.ContentType, result.FileName);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("documents/updateStatusDocument/{documentId}")]
        public async Task<IActionResult> UpdateStatusDocument(int documentId)
        {
            var userId = this.GetUserId();
            var command = new UpdateStatusDocumentCommand { DocumentId = documentId, UpdatedBy = userId };
            var result = await this._mediator.Send(command);
            return this.Ok(result);
        }

        private int GetUserId()
        {
            var userIdClaim = this.User.FindFirst("UserId") ?? this.User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found or invalid in token.");
            }

            return userId;
        }
    }
}
