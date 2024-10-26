namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestReasonByUserId;
    using BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestStatusByUserId;
    using BackEndAje.Api.Application.Dtos;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetOrderRequestStatusByUserIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("order-request/status")]
        public async Task<IActionResult> GetOrderRequestStatusByUserId([FromQuery]int? regionId, [FromQuery]int? zoneId, [FromQuery]int? route, [FromQuery]int? month, [FromQuery]int? year)
        {
            var userId = this.GetUserId();
            var query = new GetOrderRequestStatusByUserIdQuery(userId, regionId, zoneId, route, month, year);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetOrderRequestStatusByUserIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("order-request/reason")]
        public async Task<IActionResult> GetOrderRequestReasonByUserId([FromQuery]int? regionId, [FromQuery]int? zoneId, [FromQuery]int? route, [FromQuery]int? month, [FromQuery]int? year)
        {
            var userId = this.GetUserId();
            var query = new GetOrderRequestReasonByUserIdQuery(userId, regionId, zoneId, route, month, year);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
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