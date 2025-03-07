namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Locations.Queries.GetCedis;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisById;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisByRegionId;
    using BackEndAje.Api.Application.Locations.Queries.GetCedisByUserId;
    using BackEndAje.Api.Application.Locations.Queries.GetRegions;
    using BackEndAje.Api.Application.Locations.Queries.GetZoneByCediId;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class LocationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetRegionsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetRegions")]
        public async Task<IActionResult> GetRegions()
        {
            var query = new GetRegionsQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetCedisByRegionIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetCedisByRegionId/{regionId}")]
        public async Task<IActionResult> GetCedisByRegionId(int regionId)
        {
            var query = new GetCedisByRegionIdQuery
            {
                RegionId = regionId,
            };
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetZoneByCediIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetZoneByCediId")]
        public async Task<IActionResult> GetZoneByCediId(int? cediId)
        {
            var query = new GetZoneByCediIdQuery
            {
                CediId = cediId,
            };
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetCedisByIdResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetCediById/{cediId}")]
        public async Task<IActionResult> GetCedisById(int cediId)
        {
            var query = new GetCedisByIdQuery(cediId);
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetCedisByUserIdResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetCedisByUserId")]
        public async Task<IActionResult> GetCedisByUserId(int userId)
        {
            var query = new GetCedisByUserIdQuery(userId);
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetCedisResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetCedis")]
        public async Task<IActionResult> GetCedis()
        {
            var query = new GetCedisQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }
    }
}
