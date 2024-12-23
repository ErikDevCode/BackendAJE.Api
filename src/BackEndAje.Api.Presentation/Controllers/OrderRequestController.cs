namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.OrderRequestAssets.Commands.AssignAssetsToOrderRequest;
    using BackEndAje.Api.Application.OrderRequestAssets.Commands.DeleteAssetToOrderRequest;
    using BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById;
    using BackEndAje.Api.Application.OrderRequests.Commands.BulkInsertOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Commands.CreateOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Commands.DeleteOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Commands.UpdateStatusOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Documents.Commands.CreateDocumentByOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Documents.Commands.DeleteDocumentByOrderRequest;
    using BackEndAje.Api.Application.OrderRequests.Queries.ExportOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetOrderRequestById;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingAssetsByOrderRequestId;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetTrackingByOrderRequestId;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
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
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkCreatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetAllOrderRequestsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAllOrderRequests(
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] int? ClientCode = null,
            [FromQuery] int? StatusCode = null,
            [FromQuery] int? ReasonRequestId = null,
            [FromQuery] int? CediId = null,
            [FromQuery] int? RegionId = null,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null)
        {
            var query = new GetAllOrderRequestsQuery(pageNumber, pageSize, ClientCode, StatusCode, ReasonRequestId, CediId, RegionId, StartDate, EndDate);
            var roles = await this._mediator.Send(query);
            return this.Ok(new Response { Result = roles });
        }

        [HttpGet("{orderRequestId}")]
        [ProducesResponseType(typeof(GetOrderRequestByIdResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderRequestById(int orderRequestId)
        {
            var result = await this._mediator.Send(new GetOrderRequestByIdQuery(orderRequestId));
            return this.Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("delete")]
        public async Task<IActionResult> DeleteOrderRequest([FromBody] DeleteOrderRequestCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("updateStatusOrderRequest")]
        public async Task<IActionResult> UpdateStatusOrderRequest([FromBody] UpdateStatusOrderRequestCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetTrackingByOrderRequestIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.NotFound)]
        [Route("tracking/{orderRequestId}")]
        public async Task<IActionResult> GetTrackingByOrderRequestId(int orderRequestId)
        {
            var result = await this._mediator.Send(new GetTrackingByOrderRequestIdQuery(orderRequestId));
            return this.Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("documents/create")]
        public async Task<IActionResult> CreateDocumentByOrderRequest([FromForm] CreateDocumentByOrderRequestCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkCreatedResult });
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("documents/delete")]
        public async Task<IActionResult> DeleteDocumentByOrderRequest([FromBody] DeleteDocumentByOrderRequestCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpGet("documents/by-orderrequestid/{orderRequestId}")]
        public async Task<IActionResult> DocumentsByOrderRequestId(int orderRequestId)
        {
            var documents = await this._mediator.Send(new GetOrderRequestDocumentByIdQuery(orderRequestId));
            return this.Ok(new Response { Result = documents });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("assets/assign-assets")]
        public async Task<IActionResult> AssignAssetsToOrderRequest([FromBody] AssignAssetsToOrderRequestCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkAssignResult });
        }

        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [Route("assets/delete-asset")]
        public async Task<IActionResult> DeleteAssetToOrderRequest([FromBody] DeleteAssetToOrderRequestCommand command)
        {
            await this._mediator.Send(command);
            return this.Ok(new { Message = ConstName.MessageOkUpdatedResult });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetTrackingAssetsByOrderRequestIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IDictionary<string, string>), (int)HttpStatusCode.NotFound)]
        [Route("tracking/assets/{orderRequestId}")]
        public async Task<IActionResult> GetTrackingAssetsByOrderRequestId(int orderRequestId)
        {
            var result = await this._mediator.Send(new GetTrackingAssetsByOrderRequestIdQuery(orderRequestId));
            return this.Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file, int reasonRequest)
        {
            if (file == null || file.Length == 0)
            {
                return this.BadRequest(new { Message = ConstName.MessageNotSelectFileResult });
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var command = new BulkInsertOrderRequestsCommand
            {
                File = memoryStream.ToArray(),
                ReasonRequest = reasonRequest,
            };
            await this._mediator.Send(command);

            return this.Ok(new { Message = ConstName.MessageOkBurkUploadResult });
        }

        [HttpGet]
        [Route("export")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ExportOrderRequests(
            [FromQuery] int? ClientCode = null,
            [FromQuery] int? StatusCode = null,
            [FromQuery] int? ReasonRequestId = null,
            [FromQuery] int? CediId = null,
            [FromQuery] int? RegionId = null,
            [FromQuery] DateTime? StartDate = null,
            [FromQuery] DateTime? EndDate = null)
        {
            try
            {
                var query = new ExportOrderRequestsQuery(ClientCode, StatusCode, ReasonRequestId, CediId, RegionId, StartDate, EndDate);
                var fileContent = await this._mediator.Send(query);
                var fileName = $"Solicitudes_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return this.File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Message = ex.Message });
            }
        }
    }
}
