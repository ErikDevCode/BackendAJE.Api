namespace BackEndAje.Api.Presentation.Controllers
{
    using System.Net;
    using BackEndAje.Api.Application.Dtos;
    using BackEndAje.Api.Application.Ubigeo.Queries.GetDepartments;
    using BackEndAje.Api.Application.Ubigeo.Queries.GetDistrictsByProvinceId;
    using BackEndAje.Api.Application.Ubigeo.Queries.GetProvincesByDepartmentId;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class UbigeoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UbigeoController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetDepartmentsResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var query = new GetDepartmentsQuery();
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetProvincesByDepartmentIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetProvinces/{departmentId}")]
        public async Task<IActionResult> GetProvinces(string departmentId)
        {
            var query = new GetProvincesByDepartmentIdQuery(departmentId);
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GetDistrictsByProvinceIdResult>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Route("GetDistricts/{provinceId}")]
        public async Task<IActionResult> GetDistricts(string provinceId)
        {
            var query = new GetDistrictsByProvinceIdQuery(provinceId);
            var results = await this._mediator.Send(query);
            return this.Ok(new Response { Result = results });
        }
    }
}
