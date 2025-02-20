namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dashboard.Queries.GetAssetsFromOrderRequestStatusAttendedByUserId;
    using BackEndAje.Api.Application.Dashboard.Queries.GetCensusOrderRequestByFilters;
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
        public async Task<IActionResult> GetOrderRequestStatusByUserId([FromQuery]int? regionId, [FromQuery]int? cediId, [FromQuery]int? zoneId, [FromQuery]int? route, [FromQuery]int? month, [FromQuery]int? year)
        {
            var query = new GetOrderRequestStatusByUserIdQuery(regionId, cediId, zoneId, route, month, year);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetOrderRequestStatusByUserIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("order-request/reason")]
        public async Task<IActionResult> GetOrderRequestReasonByUserId([FromQuery]int? regionId, [FromQuery]int? cediId, [FromQuery]int? zoneId, [FromQuery]int? route, [FromQuery]int? month, [FromQuery]int? year)
        {
            var query = new GetOrderRequestReasonByUserIdQuery(regionId, cediId, zoneId, route, month, year);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAssetsFromOrderRequestStatusAttendedByUserIdResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("order-request/assets-attended")]
        public async Task<IActionResult> GetAssetsFromOrderRequestStatusAttendedByUserId([FromQuery]int? regionId, [FromQuery]int? cediId, [FromQuery]int? zoneId, [FromQuery]int? route, [FromQuery]int? month, [FromQuery]int? year)
        {
            var query = new GetAssetsFromOrderRequestStatusAttendedByUserIdQuery(regionId, cediId, zoneId, route, month, year);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetCensusOrderRequestByFiltersResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("census/asset")]
        public async Task<IActionResult> GetCensusOrderRequestByFilters([FromQuery]int? regionId, [FromQuery]int? cediId, [FromQuery]int? zoneId, [FromQuery]int? route, [FromQuery]int? month, [FromQuery]int? year)
        {
            var query = new GetCensusOrderRequestByFiltersQuery(regionId, cediId, zoneId, route, month, year);
            var clients = await this._mediator.Send(query);
            return this.Ok(new Response { Result = clients });
        }
    }
}
